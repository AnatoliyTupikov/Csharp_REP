using HandPipeline;
using System.Text;
using System.Threading;
using static System.Net.WebRequestMethods;

namespace HttpRequests
{
    internal class HandPipeline
    {
        static async Task Main(string[] args)
        {
            // It's possible to create a pipeline for HTTP requests. Good practice to use IHttpClientFactory for this, but it requires cookies using, the IHttpClientFactory does not pass. For more information: https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory
            // Pipeline is a sequence of objects of inherited DelegatingHandler class, which overide SendAsync method. Each handler can process the request and response messages. For examle, lower the next pipeline was developed:
            //HttpClient
            //   ↓
            //LoggingHandler
            //   ↓
            //AuthenticationHandler
            //   ↓
            //RetryHandler
            //   ↓
            //HttpClientHandler (final handler wwich sends the request)
            //   ↑
            //The answer goes back the same way. Like recursive calls.

            // Building of a pipline makes from the bottom to the top:
            var httpHandler = new HttpClientHandler();

            var retryHandler = new RetryHandler() { InnerHandler = httpHandler };            

            var authHandler = new BaseAuthHandler() { InnerHandler = retryHandler };

            var loggingHandler = new LoggingHandler() { InnerHandler = authHandler };

            var MainClient = new HttpClient(loggingHandler);

            //Create a request for BaseAuthHandler:
            string Username = "postman", Password = "password";
            using HttpRequestMessage AuthReqMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("https://postman-echo.com/basic-auth"));
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));
            AuthReqMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);

            //Create main request:
            using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "https://postman-echo.com/headers");
            //Provide a request meta data for pipeline hendlers (in the case: for BaseAuthHandler):
            req.Options.Set<HttpRequestMessage>(new HttpRequestOptionsKey<HttpRequestMessage>("auth_request"), AuthReqMessage);

            using var response = await MainClient.SendAsync(req);

            Console.WriteLine("Main Request headers: " + await response.Content.ReadAsStringAsync());
            // After the request, we will see in the console, that each handler has processed the request and response messages.

            //Here, after the request, we exract the returned value from the pipeline:
            String? pipeline_returned_value;
            HttpRequestOptionsKey<String> pipeline_returned_key = new HttpRequestOptionsKey<String>("pipeline_return");
            if (req.Options.TryGetValue(pipeline_returned_key, out pipeline_returned_value)) 
            {
                Console.WriteLine("Pipeline returned value: " + pipeline_returned_value);
            }




        }
    }
}
