using Common.Models;

namespace Localos.Communication.Abstraction;

public interface ICommunicator: IDisposable
{
    event Func<RequestSchema, Task>? OnRequest;
    Task Listen();
}
