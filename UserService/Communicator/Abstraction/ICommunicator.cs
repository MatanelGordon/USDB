using Common.Models;
using UserService.Communicator.Model;

namespace UserService.Communicator.Abstraction;

public interface ICommunicator: IDisposable
{
    event Func<OnRequestPayload, Task>? OnRequest;

    Task Listen();
}
