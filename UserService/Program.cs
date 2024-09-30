using Common.Compression;
using Common.Compression.Abstraction;
using Common.Models;
using Common.Protocol;
using Common.Protocol.Abstraction;
using Common.Serializer;
using Common.Serializer.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService;
using UserService.Cnc;
using UserService.Communicator;
using UserService.Communicator.Abstraction;
using UserService.Config;
using UserService.RegisterService;
using UserService.RegisterService.Abstraction;
using UserService.RequestController;
using UserService.RequestController.Abstraction;
using UserService.Storage;
using UserService.Storage.Abstraction;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(app =>
{
    var basePath = Path.GetDirectoryName(Directory.GetCurrentDirectory())!;
    var fullPath = Path.GetFullPath(Path.Combine(basePath, "../../"));

    app.SetBasePath(fullPath).AddJsonFile("config.json");
});

builder.ConfigureServices((context, services) =>
{

    var network = context.Configuration.GetSection("Network");
    services.Configure<NetworkConfigSchema>(network);

    var db = context.Configuration.GetSection("DB");
    services.Configure<DBConfigSchema>(db);

    var identity = context.Configuration.GetSection("Identity");
    services.Configure<IdentityConfigSchema>(identity);

    services.AddSingleton<IStorage, StorageRouter>();

    services.AddSingleton<HttpCncService>();
    services.AddSingleton<ISerializer, JsonSerializer>();
    services.AddSingleton<IProtocol, DefaultProtocol>();
    services.AddSingleton<ICommunicator, TcpCommunicator>();
    services.AddSingleton<IRegisterService, HttpRegisterService>();

    services.AddSingleton<MainController>(services =>
    {
        var mapping = new Dictionary<RequestMethod, IRequestController>()
        {
            [RequestMethod.GET] = new GetController(),
            [RequestMethod.ADD] = new AddController(),
            [RequestMethod.DELETE] = new DeleteController(),
        };

        return new MainController(mapping);
    });

    services.AddHostedService<Startup>();
});

await builder.Build().RunAsync();