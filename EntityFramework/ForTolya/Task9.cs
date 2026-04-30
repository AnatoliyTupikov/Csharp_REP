using System;
using System.Collections.Generic;
using System.Text;

namespace ForTolya
{
    internal class Task9
    {
        class A : IDisposable 
        {
            public A() 
            {
                throw new Exception("A::Exception");
            }
            public void Do() 
            {
                Console.WriteLine("A::Do()");
            }
            public void Dispose() 
            {
                Console.WriteLine("A::Dispose()");
            }
        }

        internal static void main(string[] args) 
        {
            try
            {
                using (A a = new A()) //Т.к. объект "A" не был создан, то using не отработет и Dispose не будет вызван 
                {
                    a.Do();
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
