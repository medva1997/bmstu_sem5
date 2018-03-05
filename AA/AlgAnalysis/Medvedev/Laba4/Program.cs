using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Laba4
{
    class Program
    {
        private static double[,] Generator(int m, int n)
        {
            Random random = new Random();
            double[,] numArray = new double[m, n];
            int maxValue = Math.Max(m, n);
            for (int index1 = 0; index1 < m; ++index1)
            {
                for (int index2 = 0; index2 < n; ++index2)
                    numArray[index1, index2] = random.Next(-maxValue, maxValue);
            }

            return numArray;
        }

        private static void TestMult(double[,] matrix1, double[,] matrix2, int nTimes)
        {
          
            List<long> times = new List<long>();

            long num = ComputeAverageExecutionTime(()=>Multiplication.BaseMultiplication(matrix1, matrix2),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.VinogradMultiplication(matrix1, matrix2),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.BetterVinogradMultiplication(matrix1, matrix2),nTimes);
            times.Add(num);
            
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelBaseMultiplication(matrix1, matrix2,1),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelBaseMultiplication(matrix1, matrix2,2),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelBaseMultiplication(matrix1, matrix2,3),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelBaseMultiplication(matrix1, matrix2,4),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelBaseMultiplication(matrix1, matrix2,8),nTimes);
            times.Add(num);
            
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelVinogradMultiplication(matrix1, matrix2,1),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelVinogradMultiplication(matrix1, matrix2,2),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelVinogradMultiplication(matrix1, matrix2,3),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelVinogradMultiplication(matrix1, matrix2,4),nTimes);
            times.Add(num);
            num=ComputeAverageExecutionTime(()=>Multiplication.ParallelVinogradMultiplication(matrix1, matrix2,8),nTimes);
            times.Add(num);
            

            


            Console.Write($"{matrix1.GetLength(0)}x{matrix1.GetLength(1)};");
            foreach (var variable in times)
            {
                Console.Write($"{variable};");
            }
            Console.Write("\n");
        }


        private static long ComputeAverageExecutionTime(Action func,int nTimes)
        {
            long num = 0;
            Stopwatch stopwatch = new Stopwatch();
            for (int index = 0; index <= nTimes; ++index)
            {
                stopwatch.Reset();
                stopwatch.Start();
                func();
                stopwatch.Stop();
                num += stopwatch.ElapsedMilliseconds;
            }

            return num / nTimes;
        }

        private static void Test()
        {
            for (int index = 1; index < 12; ++index)
                TestMult(Generator(index * 100, index * 100),
                    Generator(index * 100, index * 100), 10);
            for (int index = 1; index < 12; ++index)
            {
                int n = index * 100 + 1;
                TestMult(Generator(n, n), Generator(n, n), 10);
            }
        }

        static void Main()
        {
            Test();
            Console.ReadKey();
            Console.WriteLine("Hello World!");
        }
    }
}