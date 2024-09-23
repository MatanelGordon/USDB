using UserService.Communicator.Abstraction;
using UserService.RequestController;
using UserService.Storage.Abstraction;
using Microsoft.Extensions.Hosting;

namespace UserService;

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
