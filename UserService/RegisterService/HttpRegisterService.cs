using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Cnc;
using UserService.Config;
using UserService.RegisterService.Abstraction;

namespace UserService.RegisterService;

internal class HttpRegisterService(HttpCncService cncHttp, IOptions<IdentityConfigSchema> identityConfig, IOptions<NetworkConfigSchema> netconfig, ILogger<HttpRegisterService> logger) : IRegisterService
{
    public async Task Register()
    {
        try
        {
            var name = identityConfig.Value.Name;
            var port = netconfig.Value.Port;
            var result = await cncHttp.Http.GetAsync($"/Api/UserService/Register?user={name}&port={port}");
            result.EnsureSuccessStatusCode();
            logger.LogInformation($"Registration Completed as {name}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Registration Failed - {ex.Message}");
            throw;
        }
    }
}
