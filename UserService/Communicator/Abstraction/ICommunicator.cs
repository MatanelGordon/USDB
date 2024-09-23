using Common.Models;

namespace USDB.Communication.Abstraction;

public interface ICommunicator: IDisposable
{
    event Func<RequestSchema, Task>? OnRequest;
    Task Listen();
}
