using Common.Models;
using UserService.Storage.Abstraction;

namespace UserService.PayloadController.Abstraction;

internal interface IRequestController
{
    Task Handle(RequestSchema request, IStorage storage);
}
