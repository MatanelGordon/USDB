namespace UserService;

internal record ConfigSchema
{
    public required int Port { get; init; }
    public required string Directory { get; init; }
    public required int Limit { get; init; }
    public required int MemoryStorageLimit { get; init; }
    public required string Address { get; init; }

}