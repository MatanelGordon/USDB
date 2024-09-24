namespace UserService.Config;

internal record DBConfigSchema
{
    public required string Directory { get; init; }
    public required int Limit { get; init; }
    public required int MemoryStorageLimit { get; init; }
}
