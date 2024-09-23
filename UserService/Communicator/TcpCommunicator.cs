using Common.Models;
using Common.Protocol.Abstraction;
using Common.Serializer.Abstraction;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using UserService.Communicator.Abstraction;

namespace UserService.Communicator;

class TcpCommunicator(
    ISerializer serializer,
    IProtocol protocol,
    IOptions<ConfigSchema> config
    ) : ICommunicator
{
    private readonly CancellationTokenSource cancellationToken = new();

    public event Func<RequestSchema, Task>? OnRequest;

    public void Dispose()
    {
        cancellationToken.Cancel();
    }

    public async Task Listen()
    {
        try
        {
            Console.WriteLine($"Listening on {config.Value.Address}:{config.Value.Port}");
            TcpListener listener = new TcpListener(IPAddress.Parse(config.Value.Address), config.Value.Port);

            listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(cancellationToken.Token);

                var _ = HandleClientAsync(client);
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

        while (true)
        {
            if (!client.Connected) return;

            var rawData = await protocol.Unwrap(stream);
            var payload = await serializer.Deserialize<RequestSchema>(rawData);

            if (payload is null)
            {
                Console.WriteLine("Error Deserializing payload data");
                continue;
            }

            OnRequest?.Invoke(payload);
        }
    }
}
