using System;
using System.Collections.Generic;
using System.Text;

namespace ForTolya
{
    internal class Task3
    {
        class MyException : Exception 
        {
            
        }

        internal static void main (string[] args) 
        {
            try
            {
                throw new MyException();
            }
            catch (MyException)
            {
                Console.WriteLine("MyException");
            }
            catch (Exception) 
            {
                Console.WriteLine("Exception");
            }
            catch // Перехватывает любое выброшенное исключение, также не наследованное от Exception, например из неуправляемого кода. (Используется редко)
            {
                Console.WriteLine("Catch");
            }
            finally 
            {
                Console.WriteLine("Finally");
            }
        }
    }
}
