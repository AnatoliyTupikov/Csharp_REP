using System;
using System.Net;

namespace HttpRequests
{
    internal class Cookies
    {
        static async Task Main(string[] args)
        {
            //Cookies are small pieces of data that server can send to client, and client store them and send back to server with next requests to the same server.
            // Cookies as an object belong to SocketsHttpHandler object, not to HttpClient.
            CookieContainer cookieContainer = new CookieContainer();
            // It stores cookies for HTTP request for defined URIs.

            HttpClientHandler handler = new HttpClientHandler(); // Here we use HttpClientHandler instead of SocketsHttpHandler, because we don't need low level settings.
            handler.CookieContainer = cookieContainer;
            handler.UseCookies = true; // It's true by default, but just for demonstration.
            HttpClient client = new HttpClient(handler);// We can't get cookies from default HttpClient (created without handler),
                                                        // so we need to create sepparate CookieContainer. Assign it to HttpClientHandler and use this handler to create HttpClient.

            await client.GetAsync("https://postman-echo.com/cookies/set?foo1=bar1&foo2=bar2");

            Console.WriteLine("Cookies:");
            foreach (Cookie cookie in cookieContainer.GetCookies(new Uri("https://postman-echo.com")))
            // Make attention to URI:
            //      1. Cookies are stored per domain. So we need to specify the domain to get cookies for it.
            //      2. It needs of server name, no full method path, because cookies are stored for the whole domain.
            {
                Console.WriteLine($"\t{cookie.Name} = {cookie.Value}");
            }

            //So as we can see, the response isn't needed to read cookies, because cookies list is an object of Http session, not of request/response.
        }
    }
}
