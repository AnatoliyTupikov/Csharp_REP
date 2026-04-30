namespace IsBackground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread mainThread = Thread.CurrentThread;
            Console.WriteLine($"Main thread: {mainThread.Name} id: {mainThread.ManagedThreadId}");

        
            Thread thread1 = new Thread(() => {               
                Thread.Sleep(1000);
                Console.WriteLine("Thread1 is done!");
                
            });
            Console.WriteLine($"Thread1 status: {thread1.ThreadState}");
            thread1.IsBackground = true;
            //Now the application will not to wait end work of thread1. And when all not background threads will be done (only main thread in this case), the application will be closed
            Console.WriteLine($"Thread1 start");
            thread1.Start();
            // thread1.Join(); // Blocks the calling thread (e.g., main thread) until the thread (thread1) has finished execution. It doesn't metter, IsBackground thread1 or not
            Console.WriteLine($"Thread1 status: {thread1.ThreadState}");           
        }
    }
}
