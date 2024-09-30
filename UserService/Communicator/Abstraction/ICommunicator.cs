using Common.Models;
using UserService.Communicator.Model;

namespace UserService.Communicator.Abstraction;

public interface ICommunicator
{
    event Func<OnRequestPayload, Task>? OnRequest;

    Task Listen(CancellationToken cancellationToken);
}
