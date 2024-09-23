using Common;
using Common.Compression;
using Common.Compression.Abstraction;
using Common.Protocol;
using Common.Protocol.Abstraction;
using Common.Serializer.Abstraction;
using UserService;
using UserService.Communicator;
using UserService.Communicator.Abstraction;
using UserService.Serializer;
using UserService.Storage;
using UserService.Storage.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(app =>
{
    var basePath = Path.GetDirectoryName(Directory.GetCurrentDirectory())!;
    var fullPath = Path.GetFullPath(Path.Combine(basePath, "../../"));

    app.SetBasePath(fullPath).AddJsonFile("config.json");
});

builder.ConfigureServices((context, services) =>
{
    var config = context.Configuration.GetSection("Config");
    services.Configure<ConfigSchema>(config);

    var compression = context.Configuration.GetSection("Compression");
    services.Configure<CompressionConfigSchema>(config);

    services.AddSingleton<IStorage, StorageRouter>();

    services.AddSingleton<ICompression, ZstdCompression>();
    services.AddSingleton<ISerializer, JsonSerializer>();
    services.AddSingleton<IProtocol, DefaultProtocol>();
    services.AddSingleton<ICommunicator, TcpCommunicator>();
    services.AddHostedService<Startup>();
});

await builder.Build().RunAsync();