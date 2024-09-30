using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using UserService.Config;

namespace UserService.Cnc;

class HttpCncService: IDisposable
{
    public HttpClient Http { get; }
    public HttpCncService(IOptions<NetworkConfigSchema> netConfig)
    {
        var config = netConfig.Value;
        var builder = new UriBuilder();

        builder.Port = config.CNC.Port;
        builder.Host = config.CNC.Address;
        builder.Scheme =  config.CNC.IsSecure ? "https" : "http";

        Http = new HttpClient()
        {
            BaseAddress = builder.Uri
        };
        
        Http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows; Windows NT 10.2;; en-US) Gecko/20130401 Firefox/53.0");
        Http.DefaultRequestHeaders.Accept.ParseAdd("*/*");
    }

    public void Dispose()
    {
        Http.Dispose();
    }
}
