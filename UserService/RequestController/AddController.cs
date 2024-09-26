using System.Text;
using Common.Compression.Abstraction;
using Common.Models;
using UserService.RequestController.Abstraction;
using UserService.RequestController.Model;

namespace UserService.RequestController;

internal class AddController(ICompression compression): IRequestController
{
    public async Task Handle(ControllerContext ctx)
    {
        var status = ResponseStatus.Success;
        var content = Array.Empty<byte>();
        
        try
        {
            var body = ctx.Request.Body;
            
            if(body is null) throw new NullReferenceException("Body is null");
            var compressed = await compression.Compress(body);
            await ctx.Storage.AddObject(ctx.Request.Id, compressed);
        }
        catch (Exception ex)
        {
            status = ResponseStatus.GeneralError;
            content = Encoding.UTF8.GetBytes(ex.Message);
        }

        var response = new ResponseSchema()
        {
            Id = ctx.Request.Id,
            Content = content,
            ResponseStatus = status,
        };
        
        await ctx.Send(response);
    }
}
