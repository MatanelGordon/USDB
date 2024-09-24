using Common.Models;
using UserService.Storage.Abstraction;
using UserService.Communicator.Model;

namespace UserService.RequestController.Abstraction;

internal interface IRequestController
{
    Task Handle(OnRequestPayload payload, IStorage storage);
}
