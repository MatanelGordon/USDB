using System.Net;

namespace Common.Models;

public record ResponseSchema
{
    public required ResponseStatus ResponseStatus { get; init; }
    public required byte[] Content { get; init; }
    public string From { get; init; } = Dns.GetHostName();
    public required string Id { get; init; }
}
