using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace HandPipeline
{
    internal class BaseAuthHandler : DelegatingHandler
    {

        private string Base64AuthToken(string Username, string Password)
        {
            string authInfo = $"{Username}:{Password}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Here we get meta data of the request:
            HttpRequestMessage? AuthRequest;
            HttpRequestOptionsKey<HttpRequestMessage> auth_req_key = new HttpRequestOptionsKey<HttpRequestMessage>("auth_request");
            if (request.Options.TryGetValue(auth_req_key, out AuthRequest))
            {
                HttpClient AuthHttpClient = new HttpClient();
                HttpResponseMessage AuthResponse = await AuthHttpClient.SendAsync(AuthRequest);
                try
                {
                    AuthResponse.EnsureSuccessStatusCode();
                    string body = await AuthResponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(body);
                    if (jObject.ContainsKey("authenticated")) request.Headers.Add("authenticated", jObject["authenticated"]?.ToString());
                    AuthResponse.Dispose();
                }
                catch (HttpRequestException ex) when (ex is { StatusCode: HttpStatusCode.Unauthorized })
                {
                    Console.WriteLine("Athtorization fail: " + ex.Message);
                    return AuthResponse;
                }
            }

            var resp = await base.SendAsync(request, cancellationToken);

            //also we can return any data from the pipeline, just add another option to the requestMessage object:
            request.Options.Set<String>(new HttpRequestOptionsKey<String>("pipeline_return"), "The pipeline value");
            return resp;
        }

    }
}