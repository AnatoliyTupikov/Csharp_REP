//#define WrongCase
#define BaseCase

namespace _Mutex
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x = 0; // some shared date
            Mutex mutexObj = new(); //Mutex object for capturing
            //Heavyweight way to lock object for thread synchronization in OS level (lock object for any thread, including other apps/process)

            for (int i = 1; i <= 5; i++)
            {
                Thread myThread = new(Print);
                myThread.Name = $"Thread {i}";
                myThread.Start();
            }

#if WrongCase
            void Print()
            {                
                    x = 1; // Here start the critical section, with shared data using (x variable).
                           //All threads has access to it at the same time. You can't anticipate the order of the object processing by the threads
                for (int i = 1; i <= 5 ; i++)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                        x++;
                        Thread.Sleep(100);
                    }               
            }
#endif

#if BaseCase
        
           void Print()
           {
                mutexObj.WaitOne();
                x = 1; 
                for (int i = 1; i <= 5 ; i++)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                        x++;
                        Thread.Sleep(100);
                    }
                mutexObj.ReleaseMutex();
           } 
#endif
        }
    }
}
