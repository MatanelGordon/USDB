using System.Text;
using Common.Models;
using UserService.RequestController.Abstraction;
using UserService.RequestController.Model;

namespace UserService.RequestController;

internal class DeleteController : IRequestController
{
    public async Task Handle(ControllerContext ctx)
    {
        var status = ResponseStatus.Success;

        var isExists = await ctx.Storage.ObjectExists(ctx.Request.Id);
        var content = Array.Empty<byte>();

        if (!isExists)
        {
            status = ResponseStatus.NotFound;
            content = Encoding.UTF8.GetBytes($"ID {ctx.Request.Id} not found");
        }

        try
        {
            await ctx.Storage.DeleteObject(ctx.Request.Id);
        }
        catch (Exception ex)
        {
            status = ResponseStatus.GeneralError;
            content = Encoding.UTF8.GetBytes(ex.Message);
        }

        var response = new ResponseSchema
        {
            Id = ctx.Request.Id,
            ResponseStatus = status,
            Content = content
        };
            
        await ctx.Send(response);
    }
}
