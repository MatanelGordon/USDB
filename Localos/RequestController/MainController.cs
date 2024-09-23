using Common.Models;
using Localos.PayloadController;
using Localos.PayloadController.Abstraction;
using Localos.Storage.Abstraction;

namespace Localos.RequestController;

internal class MainController : IRequestController
{
    private IDictionary<RequestMethod, IRequestController> _mapping { get; }

    public MainController(IDictionary<RequestMethod, IRequestController> mapping)
    {
        _mapping = mapping;
    }

    public MainController()
    {
        _mapping = new Dictionary<RequestMethod, IRequestController>()
        {
            [RequestMethod.ADD] = new AddController(),
            [RequestMethod.DELETE] = new DeleteController(),
            [RequestMethod.GET] = new GetController(),
        };
    }

    public Task Handle(RequestSchema request, IStorage storage)
    {
        return _mapping[request.Method].Handle(request, storage);
    }
}
