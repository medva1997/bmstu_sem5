using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Laba6
{
    class Program
    {
        private static string Generator(Random rnd)
        {
            string line = "";
            int n = rnd.Next(30);
            for (int i = 0; i < n; i++)
            {
                line += rnd.Next(9).ToString();
            }

            return line;
        }

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Hello World!");
            Random rnd = new Random();

            //Sequential_processing(Words);
            Сonveyer_processing(Words);
            //  return;  
            for (int n = 100; n < 10000000; n = (int)(n*1.2))
            {
                string[] content = new string[n];
                for (int i = 0; i < content.Length; i++)
                {
                    content[i] = Generator(rnd);
                }

                long t1 = Tester(()=>Сonveyer_processing(content),10);
                long t2 = Tester(()=>Sequential_processing(content),10);

                //Console.Clear();
                //Console.WriteLine("N= " + n);
                //Console.WriteLine("Время конвеера: " + t1);
                //Console.WriteLine("Время последовательной обработки: " + t2);
                Console.WriteLine($"{n};{t1};{t2}");
                GC.Collect();
            }

            Console.ReadKey();
        }

        private static long Tester(Func<TimeSpan> func, int nTimes)
        {
           List<long> lst= new List<long>();
            for (int i = 0; i < nTimes; i++)
            {
                TimeSpan tp = func();
                lst.Add(tp.Milliseconds);
            }

            return (long) (lst.Average());
        }
        private static readonly string[] Words = {"мама", "папа", "абрикос", "таракан", "елка", "кот"};

        private static TimeSpan Сonveyer_processing(IEnumerable<string> content)
        {
            First first = new First();
            Second second = new Second();
            Third third = new Third();

            first.SetNextСonveyer(second);
            second.SetNextСonveyer(third);

            foreach (string word in content)
            {
                first.Enqueue(word);
            }

            List<Thread> threads = new List<Thread>
            {
                new Thread(() => first.Run()),
                new Thread(() => second.Run()),
                new Thread(() => third.Run())
            };


            foreach (var thread in threads)
            {
                thread.Priority = ThreadPriority.Highest;
                thread.IsBackground = false;
            }

            Stopwatch swatch = new Stopwatch();

            swatch.Reset();
            swatch.Start();
                threads.ForEach(t => t.Start());
                threads.ForEach(t => t.Join()); //ждем выполнения всех потоков
            swatch.Stop();


            //Console.WriteLine();
            //Console.WriteLine("Время конвейера: " + swatch.Elapsed);
            return swatch.Elapsed;
        }

        private static TimeSpan Sequential_processing(IEnumerable<string> content)
        {
            First first = new First();
            Second second = new Second();
            Third third = new Third();

            first.SetNextСonveyer(second);
            second.SetNextСonveyer(third);

            foreach (var word in content)
            {
                first.Enqueue(word);
            }

            Stopwatch swatch = new Stopwatch();
            swatch.Reset();
            swatch.Start();
            first.Run();
            second.Run();
            third.Run();
            swatch.Stop();
            //Console.WriteLine();
            //Console.WriteLine("Время последовательной обработки: " + swatch.Elapsed);
            return swatch.Elapsed;
        }
    }
}