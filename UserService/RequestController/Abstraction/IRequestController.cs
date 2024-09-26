using Common.Models;
using UserService.Storage.Abstraction;
using UserService.Communicator.Model;
using UserService.RequestController.Model;

namespace UserService.RequestController.Abstraction;

internal interface IRequestController
{
    Task Handle(ControllerContext ctx);
}
