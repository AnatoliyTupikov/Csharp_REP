using System.Net;

namespace _Uri
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Here, you can research adiing to URI
            Uri link = new Uri("https://vc.dom.local/sdk/vim25/8.0.1.0/");
            Uri link2 = new Uri(link, "SessionManager/SessionManager/Login");
            Console.WriteLine(link2);

            //How sepparete URI
            Console.WriteLine("Scheme: " + link2.Scheme);
            Console.WriteLine("Host: " + link.Host);
            foreach (var seg in link2.Segments)
            {
                Console.WriteLine(seg);
            }

            //Bonus: HttpStatusCode
            var correlationId = HttpStatusCode.Unauthorized;
            Console.WriteLine("Code:" + (int)correlationId);
            Console.WriteLine("Meaning:" + correlationId);
        }
    }
}
