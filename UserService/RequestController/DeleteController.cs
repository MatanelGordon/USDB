using Common.Models;
using UserService.Communicator.Model;
using UserService.RequestController.Abstraction;
using UserService.Storage.Abstraction;

namespace UserService.PayloadController;

internal class DeleteController : IRequestController
{
    public Task Handle(OnRequestPayload payload, IStorage storage)
    {
        throw new NotImplementedException();
    }
}
