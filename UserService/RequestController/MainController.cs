using Common.Models;
using Localos.RequestController.Abstraction;
using Localos.Storage.Abstraction;
using UserService.Communicator.Model;

namespace Localos.RequestController;

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
