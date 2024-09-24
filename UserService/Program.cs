using Common.Compression;
using Common.Compression.Abstraction;
using Common.Models;
using Common.Protocol;
using Common.Protocol.Abstraction;
using Common.Serializer.Abstraction;
using Localos;
using Localos.Communication;
using Localos.Communication.Abstraction;
using Localos.RequestController;
using Localos.RequestController.Abstraction;
using Localos.Serializer;
using Localos.Storage;
using Localos.Storage.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Config;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(app =>
{
    var basePath = Path.GetDirectoryName(Directory.GetCurrentDirectory())!;
    var fullPath = Path.GetFullPath(Path.Combine(basePath, "../../"));

    app.SetBasePath(fullPath).AddJsonFile("config.json");
});

builder.ConfigureServices((context, services) =>
{
    var compression = context.Configuration.GetSection("Compression");
    services.Configure<CompressionConfigSchema>(compression);

    var network = context.Configuration.GetSection("Network");
    services.Configure<NetworkConfigSchema>(network);

    var db = context.Configuration.GetSection("DB");
    services.Configure<DBConfigSchema>(db);

    var identity = context.Configuration.GetSection("Identity");
    services.Configure<IdentityConfigSchema>(identity);

    services.AddSingleton<IStorage, StorageRouter>();

    services.AddSingleton<ICompression, ZstdCompression>(services =>
    {
        var level = compression.GetValue<int>("Level");
        return new ZstdCompression(level);
    });

    services.AddSingleton<ISerializer, JsonSerializer>();
    services.AddSingleton<IProtocol, DefaultProtocol>();
    services.AddSingleton<ICommunicator, TcpCommunicator>();
    services.AddSingleton<RegisterContoller>();

    services.AddSingleton<MainController>(services =>
    {
        var mapping = new Dictionary<RequestMethod, IRequestController>()
        {
            [RequestMethod.GET] = new GetController(),
            [RequestMethod.ADD] = new AddController(),
            [RequestMethod.DELETE] = new DeleteController(),
            [RequestMethod.REGISTER] = services.GetRequiredService<RegisterContoller>()
        };

        return new MainController(mapping);
    });

    services.AddHostedService<Startup>();
});

await builder.Build().RunAsync();