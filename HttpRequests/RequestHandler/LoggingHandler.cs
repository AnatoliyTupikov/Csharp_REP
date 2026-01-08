
namespace HttpRequests
{
    internal class LoggingHandler : DelegatingHandler // It contains InnerHandler property and overideable SendAsync/Send methods.
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request's endpoind logged: " + request.RequestUri); // Processing of request
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken); // Call the next handler in the pipline, which was set in base.InnerHandler property
            Console.WriteLine("Response status code logged: " + response.StatusCode); // Processing of response
            return response;
        }
    }
}
