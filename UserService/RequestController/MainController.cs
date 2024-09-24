using Common.Models;
using UserService.RequestController.Abstraction;
using UserService.Storage.Abstraction;
using UserService.Communicator.Model;

namespace UserService.RequestController;

internal class MainController : IRequestController
{
    private IDictionary<RequestMethod, IRequestController> _mapping { get; }

    public MainController(IDictionary<RequestMethod, IRequestController> mapping)
    {
        _mapping = mapping;
    }

    public Task Handle(OnRequestPayload payload, IStorage storage)
    {
        return _mapping[payload.Request.Method].Handle(payload, storage);
    }
}
