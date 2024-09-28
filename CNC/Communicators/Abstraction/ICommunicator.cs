using Common.Models;

namespace CNC.Communicators.Abstraction
{
    public interface ICommunicator
    {
        Task<ResponseSchema> Send(RequestSchema request);
    }
}
