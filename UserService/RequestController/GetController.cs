using System.Text;
using Common.Models;
using UserService.RequestController.Abstraction;
using UserService.RequestController.Model;

namespace UserService.RequestController;

internal class GetController : IRequestController
{
    public async Task Handle(ControllerContext ctx)
    {
        ResponseSchema response;
        try
        {
            var file = await ctx.Storage.GetObject(ctx.Request.Id);

            response = new ResponseSchema()
            {
                ResponseStatus = file is null ? ResponseStatus.NotFound : ResponseStatus.Success,
                Id = ctx.Request.Id,
                Content = file ?? [],
            };
        }
        catch (Exception e)
        {
            response = new ResponseSchema()
            {
                ResponseStatus = ResponseStatus.GeneralError,
                Id = ctx.Request.Id,
                Content = Encoding.UTF8.GetBytes(e.Message),
            };
        }
        
        await ctx.Send(response);
    }
}