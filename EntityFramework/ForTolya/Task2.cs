using System;
using System.Collections.Generic;
using System.Text;

namespace ForTolya
{
    internal class Task2
    {
        class A 
        {
            public int Val;
            public A(int val) 
            { 
                Val = val; 
            }
        }

        internal static void main(string[] args)
        {
            A a1 = new A(1);
            A a2 = a1;
            a1.Val = 2;

            Console.WriteLine(a2.Val);
        }
    }
}
