using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace _HttpClientFactory
{
    internal class BaseAuthHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Auth handler processing request...");
            var resp = await base.SendAsync(request, cancellationToken);
            Console.WriteLine("Auth handler processing response...");
            return resp;
        }

    }
}
