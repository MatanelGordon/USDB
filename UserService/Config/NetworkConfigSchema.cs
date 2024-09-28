namespace UserService.Config;

internal record NetworkSchema
{
    public required int Port { get; init; }
    public required string Address { get; init; }

}

internal record NetworkConfigSchema: NetworkSchema
{
    public required NetworkSchema CNC { get; set; }
}
