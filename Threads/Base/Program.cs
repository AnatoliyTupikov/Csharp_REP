namespace Threads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //We can get object of a thread. The main thread also has it
            Thread currentThread = Thread.CurrentThread;
            Console.WriteLine("Thread name: " + currentThread.Name); //by default it's empty
            Console.WriteLine("Thread ID: " + currentThread.ManagedThreadId);
            Console.WriteLine("Is alive: " + currentThread.IsAlive);
            Console.WriteLine("Is background: " + currentThread.IsBackground); //indicates whether a thread is a background thread that does not prevent the application from exiting (true) or a foreground thread that keeps the application alive until it finishes (false).
            Console.WriteLine("Priority: " + currentThread.Priority);
            Console.WriteLine("State: " + currentThread.ThreadState);
            Console.WriteLine("Culture: " + currentThread.CurrentCulture);  //This property and the below property response for culture-sensitive operations, such as parsing and formatting, string comparison, and sorting etc.
            Console.WriteLine("UI_Culture: " + currentThread.CurrentUICulture); // for more information https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-threading-thread#culture-and-threads

        }
    }
}
