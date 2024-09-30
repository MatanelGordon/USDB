using System.Net.Sockets;
using CNC.Communicators.Abstraction;
using CNC.Communicators.Models;
using CNC.Services;
using Common.Models;
using Common.Protocol.Abstraction;
using Common.Serializer.Abstraction;

namespace CNC.Communicators;

public class TcpCommunicator(UsersStorageService userStorage, ISerializer serializer, IProtocol protocol) : ICommunicator
{
    private readonly Dictionary<string, TcpClient> _openConnections = new ();

    public async Task<ResponseSchema> MakeRequest(string user, RequestSchema request, CommunicatorSendOptions? options)
    {
        var client = await Connect(user);
        var stream = client.GetStream();

        var sentPayload = await serializer.Serialize(request);
        sentPayload = protocol.Wrap(sentPayload);

        await stream.WriteAsync(sentPayload);

        var resultRaw = await protocol.Unwrap(stream);
        var result = await serializer.Deserialize<ResponseSchema>(resultRaw);
        
        if (options is not null && !options.KeepAlive)
        {
            Disconnect(user);
        }

        return result;
    }

    private async Task<TcpClient> Connect(string user)
    {
        if (_openConnections.TryGetValue(user, out var existingConnection))
        {
            return existingConnection;
        }

        var connection = userStorage.GetRequiredHostByUser(user);

        TcpClient client = new TcpClient();

        await client.ConnectAsync(connection.Host, connection.Port);

        _openConnections.Add(user, client);

        return client;
    }

    private void Disconnect(string user)
    {
        if (!_openConnections.ContainsKey(user))
        {
            throw new Exception($"Could not find openConnection for user {user}");
        }

        var client = _openConnections[user];

        client.Close();
        _openConnections.Remove(user);
    }
}
