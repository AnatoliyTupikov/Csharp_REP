//#define BASE_IHttpClientFactory
//#define Named_IHttpClientFactory
//#define HandlersPipeLine_IHttpClientFactory
using Microsoft.Extensions.DependencyInjection;  // Microsoft.Extensions.Http nuget package for AddHttpClient()
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; // Microsoft.Extensions.Hosting for using IHost and fot the possibility to use DI at all

namespace _HttpClientFactory
{
#if BASE_IHttpClientFactory 
    public sealed class TodoClass
        (
        IHttpClientFactory httpClientFactory // IHttpClientFactory is injected via constructor
        )
    {
        public async Task<string> GetEcho()
        {
            HttpClient client = httpClientFactory.CreateClient(); // Create a new HttpClient instance using the factory

            //And then use it as usual
            using HttpResponseMessage response = await client.GetAsync("https://postman-echo.com/get?foo1=bar1&foo2=bar2");

            return await response.Content.ReadAsStringAsync();
        }
    }
#endif

#if Named_IHttpClientFactory || HandlersPipeLine_IHttpClientFactory
    public sealed class TodoClass
        (
        IHttpClientFactory httpClientFactory // IHttpClientFactory is injected via constructor
        )
    {
        public async Task<string> GetEcho()
        {
            // Create a new NAMED HttpClient instance of "EchoClient" using the factory:
            HttpClient client = httpClientFactory.CreateClient("EchoClient"); 

            //And then use it as usual
            using HttpResponseMessage response = await client.GetAsync("/get?foo1=bar1&foo2=bar2");
            //The HTTP request doesn't need to specify a hostname. The code can pass just the path, since the base address configured for the client is used.

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetRestful()
        {
            // Create a new NAMED HttpClient instance of "EchoClient" using the factory:
            HttpClient client = httpClientFactory.CreateClient("RestfulClient");

            //And then use it as usual
            using HttpResponseMessage response = await client.GetAsync("/objects");            

            return await response.Content.ReadAsStringAsync();
        }
    }
#endif

#if HandlersPipeLine_IHttpClientFactory


#endif
    internal class _HttpClientFactory
    {
        static async Task Main(string[] args)
        {

#if BASE_IHttpClientFactory
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Logging.ClearProviders(); // Mode off default logging

            builder.Services.AddHttpClient(); // Registers the IHttpClientFactory service. Now you can inject IHttpClientFactory where needed.            

            //The Factory allows to create different HttpClient instances with different configurations.
            //Itself manages the lifecycle of HttpClient instances and their SocketsHttpHandler to optimize resource usage and avoid common pitfalls like socket exhaustion.
            //Important thing, that HttpClient instances created by the factory doesn't have their own connection pools, instead they share pools managed by the factory. It leads to conflict in cookie management and other handler-level settings.
            //For more information see https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory

            using IHost host = builder.Build();

            TodoClass cl = new TodoClass(host.Services.GetRequiredService<IHttpClientFactory>()); // Request a client factory from the DI

            Console.WriteLine(await cl.GetEcho());
#endif

#if Named_IHttpClientFactory

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Logging.ClearProviders(); // Mode off default logging

            //We can also register named HttpClient instances with specific configurations, like base addresses, default headers, etc.
            //For more information see https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#named-clients

            //Registering of "EchoClient"
            builder.Services.AddHttpClient(
                "EchoClient", // Name of the Httpclient
                client =>  // Action to configure the HttpClient instance
                {
                    client.BaseAddress = new Uri("https://postman-echo.com");
                });

            //Registering of "RestfulClient"
            builder.Services.AddHttpClient(
                "RestfulClient", // Name of the Httpclient
                client =>  // Action to configure the HttpClient instance
                {
                    client.BaseAddress = new Uri("https://api.restful-api.dev");
                });

            
            using IHost host = builder.Build();

            TodoClass cl = new TodoClass(host.Services.GetRequiredService<IHttpClientFactory>()); // Request a client factory from the DI

            Console.WriteLine("Echo requests: \n\t" + await cl.GetEcho());
            Console.WriteLine("Resfull requests: \n\t" + await cl.GetRestful());
#endif

#if HandlersPipeLine_IHttpClientFactory

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Logging.ClearProviders(); // Mode off default logging

            //It's possible to add a pipeline of DelegatingHandler instances to a named HttpClient.


            //Registering of "EchoClient"
            builder.Services.AddHttpClient(
                "EchoClient", // Name of the Httpclient
                client =>  // Action to configure the HttpClient instance
                {
                    client.BaseAddress = new Uri("https://postman-echo.com");
                }).AddHttpMessageHandler(() =>
                {
                    return new LoggingHandler(); // Adding LoggingHandler to the pipeline
                }).AddHttpMessageHandler(() => 
                {
                    return new BaseAuthHandler(); // Adding BaseAuthHandler to the pipeline
                });


            
            using IHost host = builder.Build();

            TodoClass cl = new TodoClass(host.Services.GetRequiredService<IHttpClientFactory>()); // Request a client factory from the DI

            Console.WriteLine("Echo requests: \n\t" + await cl.GetEcho());

            //How we can see, sequence of handlers follows the order of their registration: the first registered - the first called (the farest from the true HttpClientHandler)
            
#endif


        }
    }

    
}
