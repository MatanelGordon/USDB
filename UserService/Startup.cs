using USDB.Communication.Abstraction;
using USDB.RequestController;
using USDB.Storage.Abstraction;
using Microsoft.Extensions.Hosting;

namespace USDB;

internal class Startup(ICommunicator communicator, IStorage storage) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Started Startup Service");
        var mainController = new MainController();

        communicator.OnRequest += request => mainController.Handle(request, storage);
        await communicator.Listen();

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("OK, Goodbye");
        communicator.Dispose();
        return Task.CompletedTask;
    }
}
