using Common.Models;
using Localos.Storage.Abstraction;
using UserService.Communicator.Model;

namespace Localos.RequestController.Abstraction;

internal interface IRequestController
{
    Task Handle(OnRequestPayload payload, IStorage storage);
}
