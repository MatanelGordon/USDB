using Common.Models;
using UserService.RequestController.Abstraction;
using UserService.RequestController.Model;

namespace UserService.RequestController;

internal class MainController : IRequestController
{
    private IDictionary<RequestMethod, IRequestController> _mapping { get; }

    public MainController(IDictionary<RequestMethod, IRequestController> mapping)
    {
        _mapping = mapping;
    }

    public Task Handle(ControllerContext ctx)
    {
        return _mapping[ctx.Request.Method].Handle(ctx);
    }
}
