using System;
using System.Collections.Generic;
using System.Text;

namespace ForTolya
{
    internal class Task5
    {
        class A 
        {
            public virtual void f() 
            {
                Console.WriteLine("A::f()");
            }
        }

        class B : A 
        {
            public void f() 
            {
                Console.WriteLine("B::f()");
            }
        }

        internal static void main(string[] args) 
        {
            A a = new B();
            a.f();
        }
    }
}
