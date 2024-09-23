using Common.Models;
using UserService.PayloadController.Abstraction;
using UserService.Storage.Abstraction;

namespace UserService.PayloadController;

internal class GetController : IRequestController
{
    public Task Handle(RequestSchema request, IStorage storage)
    {
        throw new NotImplementedException();
    }
}
