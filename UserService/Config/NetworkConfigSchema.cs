namespace UserService.Config;

internal record NetworkConfigSchema
{
    public required int Port { get; init; }
    public required string Address { get; init; }

}
