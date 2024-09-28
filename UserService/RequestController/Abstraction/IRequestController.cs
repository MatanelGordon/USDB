using UserService.RequestController.Model;

namespace UserService.RequestController.Abstraction;

internal interface IRequestController
{
    Task Handle(ControllerContext ctx);
}
