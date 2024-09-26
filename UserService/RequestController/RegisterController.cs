using Common.Models;
using UserService.RequestController.Abstraction;
using UserService.Storage.Abstraction;
using Microsoft.Extensions.Options;
using UserService.Config;
using System.Text;
using UserService.Communicator.Model;
using UserService.RequestController.Model;

namespace UserService.RequestController;

internal class RegisterController(IOptions<IdentityConfigSchema> identityConfig) : IRequestController
{
    public Task Handle(ControllerContext ctx)
    {
        var response = new ResponseSchema()
        {
            Content = Encoding.ASCII.GetBytes(identityConfig.Value.Name),
            Id = ctx.Request.Id,
            ResponseStatus = ResponseStatus.Success,
        };

        return ctx.Send(response);
    }
}
