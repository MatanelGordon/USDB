using Common.Models;
using USDB.Storage.Abstraction;

namespace USDB.PayloadController.Abstraction;

internal interface IRequestController
{
    Task Handle(RequestSchema request, IStorage storage);
}
