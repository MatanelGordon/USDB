namespace Common.Models;

public record ResponseSchema
{
    public required ResponseStatus ResponseStatus { get; init; }
    public required byte[] Content { get; init; }

    public required string Id { get; init; }
}
