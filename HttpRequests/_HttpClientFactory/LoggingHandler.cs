

namespace _HttpClientFactory
{
    internal class LoggingHandler : DelegatingHandler // It contains InnerHandler property and overideable SendAsync/Send methods.
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Logging handler processing request...");
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken); // Call the next handler in the pipline, which was set in base.InnerHandler property
            Console.WriteLine("Logging handler processing response...");
            return response;
        }
    }
}
