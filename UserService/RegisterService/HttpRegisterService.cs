using Microsoft.Extensions.Options;
using UserService.Cnc;
using UserService.Config;
using UserService.RegisterService.Abstraction;

namespace UserService.RegisterService;

internal class HttpRegisterService(HttpCncService cncHttp, IOptions<IdentityConfigSchema> identityConfig) : IRegisterService
{
    public async Task Register()
    {
        await cncHttp.HttpClient.GetAsync($"/Api/Register?user={identityConfig.Value.Name}");
    }
}
