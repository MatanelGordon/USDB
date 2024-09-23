using Common.Models;
using Localos.PayloadController.Abstraction;
using Localos.Storage.Abstraction;

namespace Localos.PayloadController;

internal class DeleteController : IRequestController
{
    public Task Handle(RequestSchema request, IStorage storage)
    {
        throw new NotImplementedException();
    }
}
