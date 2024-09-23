using Common.Models;
using Localos.Storage.Abstraction;

namespace Localos.PayloadController.Abstraction;

internal interface IRequestController
{
    Task Handle(RequestSchema request, IStorage storage);
}
