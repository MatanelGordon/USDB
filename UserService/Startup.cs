using Localos.Communication.Abstraction;
using Localos.RequestController;
using Localos.Storage.Abstraction;
using Microsoft.Extensions.Hosting;

namespace Localos;

internal class Startup(ICommunicator communicator, MainController mainController, IStorage storage) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Started Startup Service");

        communicator.OnRequest += (request, sender) => mainController.Handle(request, sender, storage);
        await communicator.Listen();

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("OK, Goodbye");
        communicator.Dispose();
        return Task.CompletedTask;
    }
}
