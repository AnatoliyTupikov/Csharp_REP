using System.Collections;
using System.Collections.ObjectModel;

namespace Iterator
{
    internal class Program
    {
        public class ForEacherEnumerator(int length) : IEnumerator<int>
        {
            readonly int _length = length;
            private int current_index = -1;

            //Instance which will be returned, while iteration
            public int Current 
            { 
                get 
                { 
                    if (current_index >= _length) throw new IndexOutOfRangeException("Enumerator is in unvalid state");
                    return current_index; 

                } 
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {                
            }

            //Method for checking to get the next element
            public bool MoveNext()
            {
                if (current_index + 1 < _length) 
                {
                    current_index++;
                    return true;
                }
                return false;
            }

            //Method for resetting the current index, but it is rarely used.
            public void Reset()
            {
                current_index = -1;
            }
        }
        public class ForEacher(int length) : IEnumerable<int>
        {
            readonly int _length = length;

            public IEnumerator<int> GetEnumerator()
            {
                return new ForEacherEnumerator(_length);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        static void Main(string[] args)
        {

            var fe = new ForEacher(3);

            //it's syntactic sugar. It call metod by line.:
            //1. Initialization "GetEnumerator()":
            //   Before iteration, the sentance request Enumerator
            //2. Check the next element "MoveNext()":   <-----------------------------------|
            //   Checking to exist the next element.                                        |
            //   If it's true move iteratation pointer, if not exit from the cycle          |
            //3. Get value from "Current" field and put it in varable                       |
            //4. Execute the body of the cycle                                              |
            //5. Return to step 2 ----------------------------------------------------------|
            foreach (var item in fe) Console.WriteLine(item);
            
        }
    }
}
