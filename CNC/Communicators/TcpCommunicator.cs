using CNC.Communicators.Abstraction;
using CNC.Communicators.Models;
using CNC.Services;
using Common.Models;
using System.Net.Sockets;

namespace CNC.Communicators;

public class TcpCommunicator(UsersStorageService userStorage) : ICommunicator
{
    private readonly Dictionary<string, TcpClient> _openConnections = new ();

    public async Task<ResponseSchema> Send(string user, RequestSchema request, CommunicatorSendOptions? options)
    {
        var client = await Connect(user);
        var stream = client.GetStream();

        var sentPayload = 
        
        
        if (options is not null && !options.KeepAlive)
        {
            client.Close();
            _openConnections.Remove(user);
        }
    }

    private async Task<TcpClient> Connect(string user)
    {
        if (_openConnections.ContainsKey(user))
        {
            return _openConnections[user];
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
