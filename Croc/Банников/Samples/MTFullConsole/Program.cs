using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTFullConsole
{
    class Program
    {
        static List<Worker> list = new List<Worker>();
        static int count = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Integrator!");
            
            // Инициализация
            for (int i = 0; i < 8; i++)
            {
                var w = new Worker();
                list.Add(w);
            }
            // Запуск всех потоков
            foreach (Worker w in list)
            {
                w.thread.Start();
            }
            Console.CancelKeyPress += Console_CancelKeyPress;
            // Ожидание завершения всех потоков
            foreach (Worker w in list)
            {
                w.thread.Join();
            }
            // Вывод результата
            foreach (Worker w in list)
            {
                Console.WriteLine(w);
            }
            Console.ReadLine();
        }
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            list[count++].thread.Abort();
            e.Cancel = true;
        }
    }
}
