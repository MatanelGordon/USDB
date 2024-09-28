using CNC.Communicators.Abstraction;
using Common.Models;
using System.Net.Sockets;

namespace CNC.Communicators
{
    public class TcpCommunicator() : ICommunicator
    {
        public async Task<ResponseSchema> Send(RequestSchema request)
        {
            throw new NotImplementedException();
        }

        private async Task<TcpClient> Connect(string address, int port = 6969)
        {
            TcpClient client = new TcpClient();

            await client.ConnectAsync(address, port);

            return client;
        }
    }
}
