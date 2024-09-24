using Common.Models;
using UserService.Communicator.Model;

namespace Localos.Communication.Abstraction;

public interface ICommunicator: IDisposable
{
    event Func<OnRequestPayload, Task>? OnRequest;

    Task Listen();
}
