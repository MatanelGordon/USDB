using Common.Models;

namespace UserService.Communicator.Abstraction;

public interface ICommunicator: IDisposable
{
    event Func<RequestSchema, Task>? OnRequest;
    Task Listen();
}
