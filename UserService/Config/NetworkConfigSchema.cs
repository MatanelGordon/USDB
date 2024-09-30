namespace UserService.Config;

internal record NetworkSchema
{
    public required int Port { get; init; }
    public required string Address { get; init; }

}

internal record CncNetworkSchema : NetworkSchema
{
    public required bool IsSecure { get; init; }

}

internal record NetworkConfigSchema: NetworkSchema
{
    public required CncNetworkSchema CNC { get; set; }
}
