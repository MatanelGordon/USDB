using Microsoft.Extensions.Hosting;
using UserService.Communicator.Abstraction;
using UserService.RegisterService.Abstraction;
using UserService.RequestController;
using UserService.RequestController.Model;
using UserService.Storage;
using UserService.Storage.Abstraction;

namespace UserService;

internal class Startup(ICommunicator communicator, MainController mainController, IStorage storage, IRegisterService registerService) : IHostedService
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
        
        // Registers To CNC
        _ = registerService.Register();
        await communicator.Listen(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("OK, Goodbye");
        return Task.CompletedTask;
    }
    
    
}
