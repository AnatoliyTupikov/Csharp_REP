using System;
using System.Collections.Generic;
using System.Text;

namespace ForTolya
{
    internal class Task10
    {
        interface I 
        {
            void f();
        }

        class A : I 
        {
            void I.f()
            {
                Console.WriteLine("I::f()");
            }

            public void f() 
            {
                Console.WriteLine("A::f()");
            }            
        }
        class B : A, I
        {
            void I.f()
            {
                Console.WriteLine("B::f()");
            }
        }

        internal static void main(string[] args) 
        {

            B b1 = new B();
            I a1 = new A();
            a1.f();

            A a2 = new A();
            I b2 = a2 as B;
            b2.f();
            
        }
    }
}
