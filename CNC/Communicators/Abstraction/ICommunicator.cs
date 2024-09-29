using CNC.Communicators.Models;
using Common.Models;

namespace CNC.Communicators.Abstraction
{
    public interface ICommunicator
    {
        Task<ResponseSchema> MakeRequest(string user, RequestSchema request, CommunicatorSendOptions? options);
    }
}
