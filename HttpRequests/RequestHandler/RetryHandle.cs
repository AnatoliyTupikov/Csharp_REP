using System;
using System.Collections.Generic;
using System.Text;

namespace HandPipeline
{
    internal class RetryHandler : DelegatingHandler
    {
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            for (int i = 0; i < 3; i++)
            {
                response = await base.SendAsync(request, cancellationToken); // We can make some end requests within one pipeline
                if (response.IsSuccessStatusCode) return response;                
            }

            Console.WriteLine("Retries failed");
            return response!;
        }
        
    }
}
