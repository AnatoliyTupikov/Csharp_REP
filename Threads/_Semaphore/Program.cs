//#define initialCount
#define maximumCount

namespace _Semaphore
{
    internal class Program
    {
        static void Print(ConsoleColor color, string message)
        {
            lock (Console.Out) // чтобы строки не перемешивались
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = color;

                Console.WriteLine(message);

                Console.ForegroundColor = old;
            }
        }

        static ConsoleColor GetColorForThread(string? name)
        {
            return name switch
            {
                "Thread 1" => ConsoleColor.Red,
                "Thread 2" => ConsoleColor.Green,
                "Thread 3" => ConsoleColor.Yellow,
                "Thread 4" => ConsoleColor.Cyan,
                "Thread 5" => ConsoleColor.Magenta,
                "Thread 6" => ConsoleColor.Blue,
                "Thread 7" => ConsoleColor.DarkRed,
                "Thread 8" => ConsoleColor.DarkGreen,
                "Thread 9" => ConsoleColor.DarkYellow,
                "Thread 10" => ConsoleColor.DarkCyan,
                _ => ConsoleColor.White
            };
        }

        static void Main(string[] args)
        {
            int maxCount = 8;
            int currentCounut = 2;
            Semaphore sem = new Semaphore(currentCounut, maxCount);
            //First parameter (initialCount) - count of threads, which can enter semaphore
            //Second parameter (maximumCount) - allow expand the semaphore to this count by Release(); method dynamicly

#if maximumCount 
            Lock obj = new Lock();
#endif

            for (int i = 1; i <= 10; i++)
            {
                Thread myThread = new(Fun);
                myThread.Name = $"Thread {i}";
                myThread.Start();
            }

#if initialCount
            void Fun()
            {
                ConsoleColor color = GetColorForThread(Thread.CurrentThread.Name);
                Print(color, $"{Thread.CurrentThread.Name} wait before semaphore");
                sem.WaitOne(); //Start of limited section. We can see, that there are not more than 2 threads at the same time in the section
                Print(color, $"{Thread.CurrentThread.Name} entered the semaphore");
                Print(color, $"{Thread.CurrentThread.Name} do...");
                Thread.Sleep(3000);
                Print(color, $"{Thread.CurrentThread.Name} finish! And it's leaving semaphore");
                sem.Release();
            }
#endif
#if maximumCount
            
            
            void Fun() 
            {
                ConsoleColor color = GetColorForThread(Thread.CurrentThread.Name);
                Print(color, $"{Thread.CurrentThread.Name} wait before semaphore");
                sem.WaitOne(); //Start of limited section. We can see, that there are not more than 2 threads at the same time in the section
                Print(color, $"{Thread.CurrentThread.Name} entered the semaphore <--");
                Print(color, $"{Thread.CurrentThread.Name} do...");
                Thread.Sleep(3000);
                lock (obj) 
                {
                    if (currentCounut < maxCount) //if count of places will be over the maxCount, the exeption will be throughted
                    {
                        Print(color, $"{Thread.CurrentThread.Name} adding additional place in semaphore! ++");
                        sem.Release();
                        currentCounut++;
                    }
                    
                }
                Print(color, $"{Thread.CurrentThread.Name} finish! And it's leaving semaphore -->");
                sem.Release();

            } 
#endif




        }
    }
}
