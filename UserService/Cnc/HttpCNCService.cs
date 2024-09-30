using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using UserService.Config;

namespace UserService.Cnc;

class HttpCncService: IDisposable
{
    public HttpClient Http { get; }

    private NetworkConfigSchema _netConfig;

    public HttpCncService(IOptions<NetworkConfigSchema> netConfig)
    {
        _netConfig = netConfig.Value;
        var builder = new UriBuilder();

        builder.Port = _netConfig.CNC.Port;
        builder.Host = _netConfig.CNC.Address;

        Http = new HttpClient()
        {
            BaseAddress = builder.Uri
        };
        
        Http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows; Windows NT 10.2;; en-US) Gecko/20130401 Firefox/53.0"));
    }

    public void Dispose()
    {
        Http.Dispose();
    }
}
