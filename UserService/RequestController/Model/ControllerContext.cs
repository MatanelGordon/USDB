using Common.Models;
using UserService.Storage.Abstraction;

namespace UserService.RequestController.Model;

public record ControllerContext
{
    public required RequestSchema Request { get; init; }
    public required Func<ResponseSchema, Task> Send { get; init; }
    public required IStorage Storage { get; init; }
};