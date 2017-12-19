using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleThread
{
    class Program
    {

        static void Main(string[] args)
        {
            WatchDog dog = new WatchDog();
            Worker[] w = { new Worker('-'), new Worker('*'), new Worker('+') };
            // Начальное состояние
            foreach (Worker a in w)
            {
                Console.WriteLine(a.N);                
            }
            // Запуск
            foreach (Worker a in w)
            {
                a.Start();
            }
            // Ожидание
            foreach (Worker a in w)
            {
                a.Join();
            }
            Console.WriteLine();
            // Вывод результата
            foreach (Worker a in w)
            {
                Console.WriteLine(a.N);
            }
            dog.Stop();
            Console.ReadLine();
        }
    }
}
