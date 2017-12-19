using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MTFullConsole
{
    public class Worker
    {
        private int xpos;
        public double a;
        public double b;
        public double N;
        public double result;

        public Thread thread;

        /// <summary>
        /// Количество запущенный экземпляров
        /// </summary>
        static private int count = 0;

        /// <summary>
        /// Объект для блокировки
        /// </summary>
        static private object locker = new object();

        static private Random r = new Random();

        public Worker()
        {
            count++;
            xpos = count * 10;

            // от 1 до 2
            a = 1 + r.NextDouble();
            // от 3 до 6
            b = 3 + 2 * r.NextDouble();
            // от 5m до 500m
            N = r.Next(50000000, 500000000);
            // Поток
            thread = new Thread(Work);
        }

        /// <summary>
        /// Деструктор класса
        /// </summary>
        ~Worker()
        {
            count--;
        }

        /// <summary>
        /// Интегрируемая функция
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double F(double x)
        {
            return 1 / (5 + 20 * x * x);
        }

        /// <summary>
        /// Выполнение вычисления
        /// </summary>
        public void Work()
        {
            double dx = (b - a) / N;
            double y = 0;
            int percent = 0;
            for (double x = a; x < b; x += dx)
            {
                y += F(x) * dx;
                // Сообщить о ходе выполнения процессе
                int newPercent = (int)((x - a) / (b - a) * 100);
                if (newPercent != percent)
                {
                    percent = newPercent;

                    lock (locker)
                    {
                        Console.SetCursorPosition(xpos, 1);
                        Console.Write("{0}%", percent);
                    }
                    // Пауза 1 мс
                    Thread.Sleep(1);
                }
            }
            result = y;
        }

        public override string ToString()
        {
            return string.Format("a = {0}, b = {1}, N = {2}, интеграл = {3}", a, b, N, result);
        }
    }
}
