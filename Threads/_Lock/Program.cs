//#define WrongCase
#define BaseCase
//#define EnterExit
//#define TryEnter
//#define EnterScope

namespace _Lock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x = 0; // some shared date
            Lock _lockObj = new(); //Object, which locked by thread. Here may be just instance of "object" class. But "Lock" class gives more functuions
            //More lighter way then mutex  to lock object, inside of one process on CLR level (one app)

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

#if BaseCase // In this case, _lockObj does not need to be of type Lock; a simple object is sufficient  (https://metanit.com/sharp/tutorial/11.4.php)
            void Print()
            {
                lock (_lockObj)  // Here start the critical section, with shared data using (x variable).
                                 // A thread tries to acquire the _lockObj. 
                                 // If the object is free, the thread enters the critical section and locks the object. 
                                 // Other threads cannot acquire it and encter the section until it is free.
                {
                    x = 1;
                    for (int i = 1; i < 5; i++)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                        x++;
                        Thread.Sleep(100);
                    }
                }//Here, the critical section is over. When the thread exit from it, to make the _lockObj is free. Another thread in the line will enter the critical section and locks the object.

            }
#endif

#if EnterExit //The same logic like in BaseCase, but via Enter/Exit methods of Lock class. Here, certanly, instance of "Lock" class is needed
            void Print()
            {
                _lockObj.Enter(); //Here start the critical section
                try //Try/catch is needed, to make free _lockObj in any case
                {
                    x = 1;
                    for (int i = 1; i <= 5; i++)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                        x++;
                        Thread.Sleep(100);
                    }
                }
                finally { _lockObj.Exit();  } //The method make the object free
                
            }
#endif

#if TryEnter //Also logic likly in "EnterExit" case, but with impotant differace: the thread doesn't wait in line and go ahead, if the objest is locked
            void Print()
            {
                if (_lockObj.TryEnter()) //If a thread was able to capture, it enters the crit. section. In opposite case, the thread will not wait and skip the section.  
                {
                    try //Try/catch is needed, to make free _lockObj in any case
                    {
                        x = 1;
                        for (int i = 1; i <= 5; i++)
                        {
                            Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                            x++;
                            Thread.Sleep(100);
                        }
                    }
                    finally { _lockObj.Exit(); } //The method make the object free
                }
                else Console.WriteLine($"I'm \"{Thread.CurrentThread.Name}\" and I skipped critical section!");                
            }
#endif

#if EnterScope //More comfortable and rcomendation by Microsoft way for threads synchronization 
            void Print()
            {
                using (_lockObj.EnterScope()) // The same logic like in BaseCase, but the constructuion vacates side resources
                {
                    x = 1;
                    for (int i = 1; i <= 5; i++)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                        x++;
                        Thread.Sleep(100);
                    }                    
                }                
            }
#endif
        }
    }
}
