#define BASE_Request_Response
//#define BASE_Connection_Pooling
//#define BASE_Default_Params
using Tools_SharedProject; //namespace from shared project


namespace HttpRequests
{

    internal class Base
    {
        static async Task Main(string[] args)
        {
            // Basic request/response example
#if BASE_Request_Response
            {

                HttpClient client = new HttpClient(); // is a collection of settings that's applied to all requests executed by that instance, and each instance uses its own connection pool, which isolates its requests from others.
                // It should be reused as much as possible, because it includes SocketsHttpHandler that manages connection pools.
                // Often reusing HttpClient to lead to the port exhaustion problem, when many connections are opened in a short time and not closed yet.
                // More information https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use

                //There is some paths to send requests, but here is an example of one of the most simple and common way: 
                //Using extension methods (GetAsync, PostAsync, etc.) of HttpClient class. For other requests methods see https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient#make-an-http-request
                using HttpResponseMessage response = await client.GetAsync("https://postman-echo.com/get?foo1=bar1&foo2=bar2");
                // It needs to dispose HttpResponseMessage object, because it contains unmanaged resources (like network connections).

                Console.WriteLine(await response.Content.ReadAsStringAsync() + "\n");// get content from response

                StringContent content = new StringContent(@"{""test"":""value""}"); // Inheritance: Object -> HttpContent -> ByteArrayContent -> StringContent
                //This realisation of HttpContent automaticly add to it's headers: "content-type": "text/plain", "content-length": "16", charset=utf-8"
                //HttpContent is abstract class that represents the content of HTTP messages. There are other realisations of HttpContent for different content types: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent?view=net-10.0

                using HttpResponseMessage respContent = await client.PostAsync("https://postman-echo.com/post", content); // Post request with "body"/content


                Console.WriteLine(await respContent.Content.ReadAsStringAsync() + "\n");//The endpoint return all parameters of request in response's body
                // For us interesting part is "data":"{\"test\":\"value\"}" - the content that we sent in the request.

            }
#endif

            // Default connection pooling example
#if BASE_Connection_Pooling
            {

                using var listener = new SocketEventListener(); //Help to monitor socket events.

                HttpClient client = new HttpClient();
                // HttpClient includes SocketsHttpHandler. The handler manages connections pools and rlated socket operations.
                // If an object isn't relayed, the HttpClient will be created with default SocketsHttpHandler settings of connection pooling.
                // One pool corresponds to one endpoint (scheme + host + port: https + postman-echo.com + 443).
                // By default, handler creates a new connection pool for each unique endpoint:

                using HttpResponseMessage response = await client.GetAsync("https://postman-echo.com/get?foo1=bar1&foo2=bar2");
                using HttpResponseMessage response2 = await client.GetAsync("https://postman-echo.com/response-headers");

                Console.WriteLine("Sequential request: " + response.StatusCode);
                Console.WriteLine("Sequential request: " + response2.StatusCode);
                // In the console output, we will see one socket open event, because  both requests are sent to the same endpoint, so they use the same connection pool and the same socket.
                Console.WriteLine("================== End of sequential requests ==================\n");

                Console.WriteLine("================== Start of multiple parallel requests ==================\n");
                // But if we send multiple requests concurrently, the handler will open multiple connections in parallel to improve perfomance.

                HttpClient client2 = new HttpClient(); // New instance for clarity

                var tasks = new[]
                {
                    // All theese requests are sent to the same endpoint, so they will use the same connection pool.
                    client2.GetAsync("https://postman-echo.com/get?foo1=bar1&foo2=bar2"),
                    client2.GetAsync("https://postman-echo.com/get?foo1=bar1&foo2=bar2"),
                    client2.GetAsync("https://postman-echo.com/response-headers"),
                    client2.GetAsync("https://postman-echo.com/get?foo1=bar1&foo2=bar2"),
                    client2.GetAsync("https://postman-echo.com/response-headers")  
                    // We will see the same count of socket open/close events as the number of requests.                
                };
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    Console.WriteLine(DateTime.Now + $" Response status code: {task.Result.StatusCode}");
                }
            }
#endif

            // Default params of requests via HttpClient
#if BASE_Default_Params
            {

                HttpClient client = new HttpClient();
                // We can specify some default parameters for all requests sent by this HttpClient instance:
                // 1. BaseAddress (the BaseAddress property is null by default)
                // 2. DefaultRequestHeaders (by default the collection has these headers: "host","accept-encoding","x-forwarded-proto") 
                // 3. Timeout (the default value is 100 seconds)
                // There are also other settings. See more details here: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-10.0

                client.BaseAddress = new Uri("https://postman-echo.com"); //Now all request via the client will be have these URL
                // It will abel to throw exception, because the BaseAddress changes are allowed only before the first request via the client.    


                using HttpResponseMessage response = await client.GetAsync("/headers"); // API method that returns headers of the request.
                // Here "/headers" will be added to the BaseAddress of the client.

                Stream str = response.Content.ReadAsStream();
                using StreamReader reader = new StreamReader(str);                
                Console.WriteLine(reader.ReadToEnd()); // Here we will see headers, although we didn't set any headers explicitly.
                Console.WriteLine("=================================================\n");


                using HttpResponseMessage response2 = await client.GetAsync("https://api.restful-api.dev/objects"); //Now we use absolute URL, so the BaseAddress of the client will be ignored.
                str = response2.Content.ReadAsStream();
                using StreamReader reader2 = new StreamReader(str);
                Console.WriteLine(reader2.ReadToEnd());
                Console.WriteLine("=================================================\n");

                client.DefaultRequestHeaders.Add("Custom-Header", "Value"); // Now all requests via the client will be have these specified header

                using HttpResponseMessage response3 = await client.GetAsync("/headers"); 
                str = response3.Content.ReadAsStream();
                using StreamReader reader3 = new StreamReader(str);
                Console.WriteLine(reader3.ReadToEnd()); // Except the default and specified headers, here will be cookie headers from before requests
                Console.WriteLine("=================================================\n");


                str.Dispose();
                
            }

#endif


        }
    }
}
