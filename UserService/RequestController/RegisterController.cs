using Common.Models;
using UserService.RequestController.Abstraction;
using UserService.Storage.Abstraction;
using Microsoft.Extensions.Options;
using UserService.Config;
using System.Text;
using UserService.Communicator.Model;

namespace UserService.RequestController;

internal class RegisterContoller(IOptions<IdentityConfigSchema> identityConfig) : IRequestController
{
    public Task Handle(OnRequestPayload payload, IStorage storage)
    {
        var response = new ResponseSchema()
        {
            Content = Encoding.ASCII.GetBytes(identityConfig.Value.Name),
            Id = payload.Request.Id,
            ResponseStatus = ResponseStatus.Success,
        };

        return payload.Send(response);
    }
}
