using CNC.Communicators.Models;
using Common.Models;

namespace CNC.Communicators.Abstraction
{
    public interface ICommunicator
    {
        Task<ResponseSchema> Send(string user, RequestSchema request, CommunicatorSendOptions? options);
    }
}
