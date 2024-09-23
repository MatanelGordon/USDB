using Common.Models;
using USDB.PayloadController.Abstraction;
using USDB.Storage.Abstraction;

namespace USDB.PayloadController;

internal class DeleteController : IRequestController
{
    public Task Handle(RequestSchema request, IStorage storage)
    {
        throw new NotImplementedException();
    }
}
