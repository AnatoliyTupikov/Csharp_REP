/*O(n) - оценка сложности алгоритма. Она показывает зависимость кол-ва операций - O от кол-ва элементов - n, проходящих через этот алгоритм. 
 *Важно отметить, что при такой оценки не учитываются константы, т.е. если каждый элемент обрабатывается каким то кол-вом операций, то это не учитыватся: O(2n) => O(n)
 *Связано это с тем, что нам интересна сама форма графика (квадратичный, экспаненциальный и пр.), а не его масштаб. 
 *Так например: 1000n (O(n) - линейная) и n^2 (O(n^2) - квадратичная )
 *Алгоритм с 1000n при маленьком кол-ве элементов будет менее продиктовен, чем n^2 аллгоритм: 1000*2=2000 > 2^2=4. Но при более больших объемах элементов в 10_000 (в рамках IT это немного): 1000*10_000 = 10_000_000 < 10_000^2 = 100_000_000 
 */

namespace Sorting
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    public class MyBenchmarks
    {
        private int[] asc_sorted_data;
        private int[] desc_sorted_data;
        private int[] no_sorted_data;

        [Params(10_000, 5000, 1000, 500, 100, 50, 10)]
        public int count;

        [GlobalSetup]
        public void Setup()
        {

            asc_sorted_data = Enumerable.Range(0, count).ToArray();

            //desc_sorted_data = Enumerable.Range(0, count).ToArray();
            //Array.Reverse(desc_sorted_data);
            desc_sorted_data = new int[count];
            for (int i = desc_sorted_data.Length - 1; i >= 0; i--) desc_sorted_data[i] = desc_sorted_data.Length - i - 1;



            var random = new Random();
            no_sorted_data = Enumerable.Range(0, count)
                                       .OrderBy(_ => random.Next())
                                       .ToArray();
        }

        //[Benchmark]
        //Benchmark for examle. It has linear dependence O(n)
        //For every addional element in the array, one iteration will be added to O
        /*
        | Method       | count | Mean              | Error          | StdDev         |
        |------------- |------ |------------------:|---------------:|---------------:|
        | SumArray     | 10    |          2.334 ns |      0.0065 ns |      0.0057 ns |
        | SumArray     | 50    |         11.373 ns |      0.0336 ns |      0.0314 ns |
        | SumArray     | 100   |         30.361 ns |      0.1807 ns |      0.1691 ns |
        | SumArray     | 500   |        132.376 ns |      0.5123 ns |      0.4541 ns |
        | SumArray     | 1000  |        259.813 ns |      0.7394 ns |      0.6916 ns |
        | SumArray     | 5000  |      1,281.570 ns |      3.7504 ns |      3.5081 ns |
        | SumArray     | 10000 |      2,565.246 ns |      7.3928 ns |      6.9152 ns |
        */
        public long SumArray()
        {
            long sum = 0;

            for (int i = 0; i < asc_sorted_data.Length; i++)
            {
                sum += asc_sorted_data[i];
            }

            return sum;
        }
        //[Benchmark]
        //BubleSort has O(n^2). But realy, the function has (n - 1)n / 2 = 0,5 * n^2 - 0,5 * n => O(n^2)
        //It has the same dependence for best and worst cases (literatly, about the same "Mean" results)
        /*
        | Method       | count | Mean              | Error          | StdDev         |
        |------------- |------ |------------------:|---------------:|---------------:|
        | BubleSorting | 10    |         19.835 ns |      0.0742 ns |      0.0658 ns |
        | BubleSorting | 50    |        494.953 ns |      1.8685 ns |      1.6564 ns |
        | BubleSorting | 100   |      2,184.245 ns |      4.7275 ns |      4.4221 ns |
        | BubleSorting | 500   |     49,379.525 ns |     47.6681 ns |     42.2565 ns |
        | BubleSorting | 1000  |    191,924.777 ns |    333.7419 ns |    312.1824 ns |
        | BubleSorting | 5000  |  4,682,471.562 ns | 13,311.2111 ns | 12,451.3151 ns |
        | BubleSorting | 10000 | 18,670,284.375 ns | 51,078.3345 ns | 47,778.7058 ns |
         */
        public void BubleSort()
        {
            int[] arr = asc_sorted_data;
            for (int i = 0; i < arr.Length; i++)
            {

                for (int j = 0; j < arr.Length - 1 - i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }

            }
        }

        [Benchmark]
        /*
        | Method                | count | Mean         | Error      | StdDev    |
        |---------------------- |------ |-------------:|-----------:|----------:|
        | QuickSortForBenchmark | 10    |     16.70 ns |   0.128 ns |  0.120 ns |
        | QuickSortForBenchmark | 50    |    118.22 ns |   0.715 ns |  0.669 ns |
        | QuickSortForBenchmark | 100   |    284.01 ns |   3.414 ns |  3.194 ns |
        | QuickSortForBenchmark | 500   |  1,946.85 ns |  11.524 ns | 10.216 ns |
        | QuickSortForBenchmark | 1000  |  3,963.68 ns |   9.651 ns |  8.556 ns |
        | QuickSortForBenchmark | 5000  | 26,600.57 ns |  57.070 ns | 53.383 ns |
        | QuickSortForBenchmark | 10000 | 56,906.67 ns | 103.751 ns | 97.049 ns |
         */
        public void QuickSortForBenchmark()
        {
            int[] arr = asc_sorted_data;
            QuickSort(arr, 0, arr.Length-1);            
        }

        private void QuickSort(int[] arr, int start, int end)
        {
            int left_p = start;
            int right_p = end;

            int lngth = right_p - left_p + 1;
            int pivot = arr[left_p + lngth / 2];
            int left_pivot = left_p + lngth / 2;
            int right_pivot = left_p + lngth / 2;

            while (left_p < left_pivot)
            {
                if (arr[left_p] > pivot)
                {
                    while (right_p > right_pivot && arr[right_p] > pivot)
                    {
                        right_p--;
                    }
                    if (right_p == right_pivot)
                    {
                        arr[right_pivot] = arr[left_p];
                        if (left_p != left_pivot - 1) arr[left_p] = arr[left_pivot - 1];
                        arr[--left_pivot] = pivot;
                        arr[--right_pivot] = pivot;
                        right_p--;
                        continue;
                    }

                    if (arr[right_p] == pivot)
                    {
                        if (right_p != right_pivot + 1) arr[right_p] = arr[right_pivot + 1];
                        arr[++right_pivot] = pivot;
                        continue;
                    }
                    else
                    {
                        (arr[left_p], arr[right_p]) = (arr[right_p], arr[left_p]);
                        left_p++;
                        right_p--;
                        continue;
                    };
                }

                if (arr[left_p] == pivot)
                {
                    if (left_p != left_pivot - 1) arr[left_p] = arr[left_pivot - 1];
                    arr[--left_pivot] = pivot;
                    continue;
                }
                left_p++;
            }
            while (right_p > right_pivot) 
            {
                if(arr[right_p] < pivot) 
                {
                    arr[left_pivot] = arr[right_p];
                    if (right_p != right_pivot + 1) arr[right_p] = arr[right_pivot + 1];
                    arr[++left_pivot] = pivot;
                    arr[++right_pivot] = pivot;
                    left_p++;
                    continue;
                }

                if (arr[right_p] == pivot)
                {
                    if (right_p != right_pivot + 1) arr[right_p] = arr[right_pivot + 1];
                    arr[++right_pivot] = pivot;
                    continue;
                }
                right_p--;
            }

            if ((left_pivot - start) > 1) QuickSort(arr, start, left_pivot - 1);
            if ((end - right_pivot) > 1) QuickSort(arr, right_pivot + 1, end);
        }
        





    }

    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MyBenchmarks>();
            //MyBenchmarks b = new MyBenchmarks();
            //b.count = 10;
            //b.Setup();
            //b.QuickSortForBenchmark();
            
            
            //b.BubleSorting();
        }
    }
}
