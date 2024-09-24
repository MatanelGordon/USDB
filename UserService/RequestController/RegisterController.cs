using Common.Models;
using Localos.RequestController.Abstraction;
using Localos.Storage.Abstraction;
using Microsoft.Extensions.Options;
using UserService.Config;
using System.Text;
using UserService.Communicator.Model;

namespace Localos.RequestController;

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
