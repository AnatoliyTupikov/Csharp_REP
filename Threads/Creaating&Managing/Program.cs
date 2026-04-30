#define UNparametrizedThreadStart
#define ParametrizedThreadStart

namespace CreaatingThreads
{
    internal class Program
    {
        static void ThreadPrint(Thread th) 
        {
            Console.WriteLine($"I processed by \"{th.Name}\" with id = {th.ManagedThreadId}");
            Console.WriteLine($"\tThread status: {th.ThreadState}");
            Console.WriteLine($"\tIs thread backgound: {th.IsBackground}");
        }
        static void Main(string[] args)
        {           
            Thread mainThread = Thread.CurrentThread;
            Console.WriteLine($"Main thread: {mainThread.Name} id: {mainThread.ManagedThreadId}");

        #if UNparametrizedThreadStart
            Thread thread1 = new Thread(() => {  //The constructor of threads takes void delegate, which will be executed when the thread will be started.                
                Thread currentThread = Thread.CurrentThread;                
                ThreadPrint(currentThread);                
            });
            thread1.Name = "Thread1";
            Console.WriteLine($"Thread1 status: {thread1.ThreadState}");            
            thread1.Start();
            thread1.Join(); //Stop the main thread, untill the thread1 will be done.
            Console.WriteLine($"Thread1 status: {thread1.ThreadState}");
#endif

#if ParametrizedThreadStart
            Thread paramThread = new Thread(x => { //This constructor takes delegate with "object" type parameter. It is not type safety.
                if (x is Thread) 
                {                    
                    ThreadPrint((Thread)x);
                }
            });
            paramThread.Name = "ParamThread";
            paramThread.Start(paramThread); //Parameter for delegate, specify just in Start method
            paramThread.Join(); //Stop the main thread, untill the thread1 will be done.
            Console.WriteLine($"paramThread status: {paramThread.ThreadState}");
        #endif
        }
    }
}
