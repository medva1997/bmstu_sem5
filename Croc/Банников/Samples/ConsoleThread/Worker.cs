using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleThread
{
    class Worker
    {
        /// <summary>
        /// Поток
        /// </summary>
        private Thread t;

        /// <summary>
        /// Простое число
        /// </summary>
        private long n;

        char ch;

        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        private static Random r = new Random();

        public long N
        {
            get { return n; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Worker(char c)
        {
            ch = c;
            n = r.Next(int.MaxValue / 2, int.MaxValue);
            t = new Thread(Process);
        }

        public void Start()
        {
            t.Start();
        }

        /// <summary>
        /// Ждать завершения потока
        /// </summary>
        public void Join()
        {
            t.Join();
            Console.Write("({0})", ch);
        }

        /// <summary>
        /// Проверка числа на простоту
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private bool IsPrime(long l)
        {
            for (int i = 2; i < l; i++)
            {
                if (l % i == 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Метод, выполняющийся в отдельном потоке
        /// </summary>
        public void Process()
        {
            while (!IsPrime(++n))
            {
                Console.Write(ch);
            }
        }
    }
}
