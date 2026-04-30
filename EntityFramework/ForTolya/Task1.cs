using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ForTolya
{
    internal class Task1
    {
        class A : IEnumerable // Реализовать интерфейс для foreach
        {
            int[] arr;
            public A(int[] arr)
            {
                this.arr = arr;
            }

            public int Length
            {
                get { return arr.Length; }
            }

            public int this[int i] // Перегрузить (хотя это не перегрузка, а определение) [] оператор.
            {
                get => arr[i];
                set => arr[i] = value;
            }

            public static implicit operator A(int[] arr) => new A(arr); //Перегружаем/определяем оператор приведения (implicit - неявный, explicit - явный )

            public IEnumerator GetEnumerator() //IEnumerable
            {
                return arr.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() //IEnumerable
            {
                return GetEnumerator();
            }
        }

        internal static void main(string[] arr) 
        {
            A a = new int[] { 1, 5, 3, 2 };

            for (int i = 0; i < a.Length; i++)
                Console.WriteLine(a[i]);

            foreach (int i in a)
                Console.WriteLine(i);
        }
    }
}
