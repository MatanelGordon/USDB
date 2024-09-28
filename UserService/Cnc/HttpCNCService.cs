using Microsoft.Extensions.Options;
using UserService.Config;

namespace UserService.Cnc;

class HttpCncService: IDisposable
{
    public HttpClient HttpClient { get; }

    private NetworkConfigSchema _netConfig;

    public HttpCncService(IOptions<NetworkConfigSchema> netConfig)
    {
        _netConfig = netConfig.Value;
        var builder = new UriBuilder();

        builder.Port = _netConfig.CNC.Port;
        builder.Host = _netConfig.CNC.Address;

        HttpClient = new HttpClient()
        {
            BaseAddress = builder.Uri
        };
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}
