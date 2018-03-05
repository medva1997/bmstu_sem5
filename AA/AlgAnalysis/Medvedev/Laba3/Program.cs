using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Laba3
{
    class Program
    {
        enum Generator
        {
            Up,
            Down,
            Random
        }

        private static Random rnd= new Random();
        static int[] GetArray(Generator type, int n)
        {
            int[] array= new int[n];

            switch (type)
            {
                    case Generator.Up :
                        for (int i = 0; i < n; i++)
                        {
                            array[i] = i - n / 2;
                        }
                        break;
                    
                    case Generator.Down:
                        for (int i = 0; i < n; i++)
                        {
                            array[i] =  n / 2 - i;
                        }
                        break;
                    case Generator.Random:
                        
                        for (int i = 0; i < n; i++)
                        {
                            array[i] = rnd.Next(n);
                        }
                        break;
                   
            }
            return array;
        }

        static List<int[]> GetTests(Generator type,int start, int step, int end)
        {
            List<int[]> lst= new List<int[]>();
            for (int i = start; i <= end; i+=step)
            {
                lst.Add(GetArray(type,i));
            }

            return lst;
        }

        static void Test(Generator type)
        {
            List<int[]> lst = GetTests(type, 100, 100, 5000);
            Stopwatch swatch = new Stopwatch();
            
            
            foreach (int[] array in lst)
            {
                long time1 = 0, time2 = 0, time3 = 0;
                
                for (int i = 0; i < 10; i++)
                {
                    int[] clone1 = (int[])(array.Clone());
                    int[] clone2 = (int[])(array.Clone());
                    int[] clone3 = (int[])(array.Clone());

                    swatch.Reset();
                    swatch.Start();
                    Sort.InsertionSort(clone1);
                    swatch.Stop();
                    time1 += swatch.ElapsedTicks;

                    swatch.Reset();
                    swatch.Start();
                    Sort.BubleSort( clone2);
                    swatch.Stop();
                    time2 += swatch.ElapsedTicks;

                    
                    swatch.Reset();
                    //Console.WriteLine($"1: {clone3[0]} {clone3[1]}");
                    swatch.Start();
                    Sort.QuickSort( clone3);
                    swatch.Stop();
                    time3 += swatch.ElapsedTicks;
                    
                    //Console.WriteLine(swatch.ElapsedTicks);
                    //Console.WriteLine($"2: {clone3[0]} {clone3[1]}");
                }

                string line = "";
                switch(type)
                {
                   case Generator.Up:
                       line = "UP";
                       break;
                   case Generator.Down:
                        line = "Down";
                        break;
                   case Generator.Random:
                        line = "Random";
                        break;
                }
                Console.WriteLine($"{line};{array.Length};{time1/10};{time2/10};{time3/10};");
                //GC.Collect();
                
            }
        }
        
        static void Main()
        {
            int[] arr = new[] {1, 5, 2, 70, 6};
            int[] arr2 = new[] {1, 5, 2, 70, 6};
            int[] arr3 = new[] {1, 5, 2, 70, 6};
            
            Sort.BubleSort( arr);
            Sort.QuickSort( arr2);
            Sort.InsertionSort( arr3);

            /*foreach (var VARIABLE in arr)
            {
                Console.WriteLine(VARIABLE);
            }
            return;
            ;*/
            
           
            
            Console.WriteLine();
            Console.WriteLine("Hello World!");
            
            Test(Generator.Up);
            Test(Generator.Down);
            Test(Generator.Random);
            Console.ReadKey();
        }
    }
}