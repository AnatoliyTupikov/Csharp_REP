namespace OverlodedBinaryOperators
{
    internal class Program
    {
        class B
        {
            public int z;
            public B(int z)
            {
                this.z = z;
            }
        }

        class A
        {
            public int i;

            public A() 
            {
                int i = 0;
                Console.WriteLine("Empty ctor " + 0);
            }

            public A( int i)
            {
                i++;
                this.i = i;
            }
            public A(B ob)
            {
                i = ob.z;
            }

            public void Print() => Console.WriteLine(this.i);

            public static A operator +(A a, A b) //In IL code below, the method marked like specialname
            /*.method public hidebysig specialname static 
                        class OverloadedOperators.Program/A 
                        op_Addition(class OverloadedOperators.Program/A a, class OverloadedOperators.Program/A b) cil managed*/
            {
                a.i = a.i + b.i;
                return a;
            }


        }
        static void Main(string[] args) 
        {
            int i1 = 2; 
            int i2 = 3; 
            int i3 = i1 + i2; 
            Console.WriteLine(i3);

            A a = new(4); 

            A b = new(6); 

            b = a + b; //In IL code below, we can see, that "op_Addition" method was called
            /*IL_001b:  call    class OverloadedOperators.Program/A OverloadedOperators.Program/A::op_Addition(class OverloadedOperators.Program/A, class OverloadedOperators.Program/A)*/
            Console.WriteLine(a);
            int show = a.i;
            Console.WriteLine(show);

            //Compiler search op_Addition method with "specialname" flag for 
        }
    }
}
