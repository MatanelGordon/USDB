using System.Net;

namespace Common.Models;

public record RequestSchema
{
    public string From { get; init; } = Dns.GetHostName();

    public byte[]? Body { get; init; }
    public required string Id { get; init; }
    public required RequestMethod Method { get; init; }
}
