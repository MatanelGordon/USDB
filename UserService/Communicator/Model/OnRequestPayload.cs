using Common.Models;

namespace UserService.Communicator.Model;

public record OnRequestPayload
{
    public required RequestSchema Request { get; init; }
    public required Func<ResponseSchema, Task> Send { get; init; }
}
