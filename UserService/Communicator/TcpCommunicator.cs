using System.Net;
using System.Net.Sockets;
using Common.Models;
using Common.Protocol.Abstraction;
using Common.Serializer.Abstraction;
using Microsoft.Extensions.Options;
using UserService.Communicator.Abstraction;
using UserService.Communicator.Model;
using UserService.Config;

namespace UserService.Communicator;

class TcpCommunicator(
    ISerializer serializer,
    IProtocol protocol,
    IOptions<NetworkConfigSchema> config
) : ICommunicator
{
    public event Func<OnRequestPayload, Task>? OnRequest;

    public async Task Listen(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"Listening on {config.Value.Address}:{config.Value.Port}");
            using var listener = new TcpListener(IPAddress.Parse(config.Value.Address), config.Value.Port);

            listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(cancellationToken);

                _ = HandleClientAsync(client);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine("Error Occurred While Listening: " + err.Message);
            throw;
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        var stream = client.GetStream();
        var sendFunc = CreateSender(stream);

        while (true)
        {
            if (!client.Connected) return;

            var rawData = await protocol.Unwrap(stream);
            var request = await serializer.Deserialize<RequestSchema>(rawData);

            if (request is null)
            {
                Console.WriteLine("Error Deserializing payload data");
                continue;
            }

            var payload = new OnRequestPayload
            {
                Request = request,
                Send = sendFunc
            };

            OnRequest?.Invoke(payload);
        }
    }

    private Func<ResponseSchema, Task> CreateSender(NetworkStream netStream)
    {
        return async req =>
        {
            var data = await serializer.Serialize(req);
            var wrappedData = protocol.Wrap(data);
            await netStream.WriteAsync(wrappedData);
        };
    }
}