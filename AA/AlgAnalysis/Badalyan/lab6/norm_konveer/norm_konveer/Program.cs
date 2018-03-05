using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace norm_konveer
{
    class Program
    {
        static Queue<int> queue1 = new Queue<int>();
        static Queue<int> queue2 = new Queue<int>();
        static Queue<int> queue3 = new Queue<int>();
        static int len = 10, schetchik = 0, pr = 500;
        static int[] input = new int[len];
        static int[] output = new int[len];
        static bool work = true;
        static Thread t1 = new Thread(f1), t2 = new Thread(f2), t3 = new Thread(f3);
        static object locker = new object();

        static void f1()
        {
            int temp = 0;
            for (int i = 0; i < len; i++)
            {
                //for (int j = 0; j < pr; j++)
                //    temp += input[i] * input[i];
                temp = input[i] * input[i];
                lock (locker)
                {
                    queue1.Enqueue(temp);
                }
                Console.WriteLine("1. Взял: " + input[i] + ", положил: " + (temp));
            }
        }

        static void f2()
        {
            int temp;
            while (work)
            {
                if (queue1.Count != 0)
                {
                    lock (locker)
                    {
                        temp = queue1.Dequeue();
                        queue2.Enqueue(temp + 3);
                    }
                    Console.WriteLine("\t2. Взял: " + temp + ", положил: " + (temp + 3));
                }
            }
        }

        static void f3()
        {
            int temp2;
            while (work)
            {
                if (queue2.Count != 0)
                {
                    lock (locker)
                    {
                        temp2 = queue2.Dequeue();
                    }
                    //for (int j = 0; j < pr; j++)
                    //    temp2 += temp2 * temp2 - 4;
                    output[schetchik] = temp2 - 10;
                    Console.WriteLine("\t\t3. Взял: " + temp2 + ", положил: " + (output[schetchik]));
                    schetchik++;
                    if (schetchik == len)
                        work = false;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding=Encoding.Unicode;
            System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch();
            Console.Write("Входной массив: ");
            int a = 2;
            for (int i = 0; i < len; i++)
            {
                input[i] = i;
                Console.Write(input[i] + " ");
            }
            Console.WriteLine("\n");
            if (a == 1)
            {
                swatch.Start();
                t1.Start();
                t2.Start();
                t3.Start();

                t1.Join();
                t2.Join();
                t3.Join();
                swatch.Stop();

                Console.Write("\nOutput: ");
                for (int i = 0; i < len; i++)
                {
                    Console.Write(output[i] + " "); 
                }
                Console.WriteLine();
                Console.WriteLine("Время конвейера: " + swatch.Elapsed);
            }
            else
            {
                int[] testoutput = new int[len];
                swatch.Start();
                int temp = 0;
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < pr; j++)
                        temp += input[i] * input[i];
                    temp += 3;
                    for (int j = 0; j < pr; j++)
                        temp += temp * temp - 4;
                    testoutput[i] = temp;
                }
                swatch.Stop();
                Console.WriteLine("Время последовательной обработки: " + swatch.Elapsed);
            }
            Console.Read();
        }
    }
}
