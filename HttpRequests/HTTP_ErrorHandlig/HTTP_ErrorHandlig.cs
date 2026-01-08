using System.Net;

namespace HttpRequests
{
    internal class HTTP_ErrorHandlig
    {
        static async Task Main(string[] args)
        {
            try 
            {
                HttpClient client = new HttpClient();
                using var resp = await client.GetAsync("https://postman-echo.com/basic-auth");
                Console.WriteLine(resp.StatusCode);
                resp.EnsureSuccessStatusCode(); // Method wich throws an exception if the HTTP response status is an error code.
            }
            catch (HttpRequestException ex) when (ex is { StatusCode: HttpStatusCode.Unauthorized })
            {
                // Handle 401
                Console.WriteLine($"Catched: {ex.Message}");
            }
        }
        // For more examples see: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient#use-http-error-handling
    }
}
