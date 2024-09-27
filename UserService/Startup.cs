using UserService.Communicator.Abstraction;
using UserService.RequestController;
using UserService.Storage.Abstraction;
using Microsoft.Extensions.Hosting;
using UserService.RequestController.Model;

namespace UserService;

internal class Startup(ICommunicator communicator, MainController mainController, IStorage storage) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Started Startup Service");

        communicator.OnRequest += payload =>
        {
            var ctx = new ControllerContext
            {
                Request = payload.Request,
                Send = payload.Send,
                Storage = storage
            };
            
            return mainController.Handle(ctx);
        };
        
        await communicator.Listen();

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("OK, Goodbye");
        communicator.Dispose();
        return Task.CompletedTask;
    }
}
