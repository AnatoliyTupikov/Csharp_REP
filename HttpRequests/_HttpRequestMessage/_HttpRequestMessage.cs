using System.Net.Http;

namespace HttpRequests
{
    internal class _HttpRequestMessage 
    {
        
        static async Task Main(string[] args)
        {
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ClientHeader", "DefHeaderValue");

            Uri uri_echo = new("https://postman-echo.com/get");
            //Class represents URL address. https://learn.microsoft.com/en-us/dotnet/api/system.uri?view=net-10.0

            using HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Get, //Object for choosing method type. https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpmethod?view=net-10.0
                                                                        uri_echo); //Istead of URI can be just string with URL here
            // The class represents a HTTP request, for more point setting. https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestmessage?view=net-10.0
            
           

            //It allows to add additional headers for example:
            request.Headers.Add("reqHeader", "AddHeaderValue");

            using HttpResponseMessage response = await client.SendAsync(request);

            Console.WriteLine(await response.Content.ReadAsStringAsync() + "\n");
            // Here we can see that, the request contains both headers: from client and from the HttpRequestMessage

            using HttpResponseMessage response2 = await client.SendAsync(request);
            // Here we will get an error, because a HttpRequestMessage object can be sent only once.
        }
    }
}
