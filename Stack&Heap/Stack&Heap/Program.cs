using System.Collections;

//ildasm "X:\VisualStudio\Repos\Csharp_REP\Stack&Heap\Stack&Heap\bin\Debug\net10.0\Boxing&Unboxing.dll"
struct Point 
{
    public int X;
    public int Y;
}


namespace Stack_Heap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ArrayList a = new ArrayList();
            Point p;
            for (int i = 0; i < 10; i++) 
            {
                p.X = p.Y = i;
                a.Add(p);
            }
        }
    }
}
