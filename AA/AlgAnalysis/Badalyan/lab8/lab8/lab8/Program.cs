using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lab8
{
    class data
    {
        public int page_ID;
        public int all_views;
        public int AVG_views;
        public int last_veiw;
        public int users;

        public data(int ID, int v1, int v2, int v3, int count)
        {
            page_ID = ID;
            all_views = v1;
            AVG_views = v2;
            last_veiw = v3;
            users = count;
        }

        public void print()
        {
            Console.WriteLine("\t\tПоток №2 - Работа закончена: web site info:");
            Console.WriteLine("\t\tПоток №2 - Работа закончена: ID = {0}, views by all time = {1}, AVG day views = {2}",
                page_ID, all_views, AVG_views);
            Console.WriteLine("\t\tПоток №2 - Работа закончена: last view time = {0}, user count = {1}", last_veiw, users);
        }

        public void some_calculations()
        {
            Console.WriteLine("\t\t\t Поток №3 - Работа закончена: Сумма просмотров = " + (all_views + AVG_views + last_veiw));
        }
    }

    class Program
    {
        static Queue<int> queue1 = new Queue<int>();
        static Queue<int> queue2 = new Queue<int>();
        static Thread t1 = new Thread(f1), t2 = new Thread(f2), t3 = new Thread(f3);
        static List<data> db = new List<data>();
        static int input_command, work_emulation = 1000000;
        static bool work = true;

        static void f1()
        {
            while (work)
            {
                string input;
                Console.WriteLine("Введите номер веб-страницы этого веб-сервера, они от 1 до 7, или 0 для завершения");
                input = Console.ReadLine();
                input_command = Convert.ToInt32(input);
                if (input_command == 0)
                {
                    Console.WriteLine("Завершение работы...");
                    work = false;
                }
                else
                {
                    if (input_command > 7 || input_command < 0)
                        Console.WriteLine("Ошибка ввода, попробуйте еще раз");
                    else
                    {
                        queue1.Enqueue(input_command);
                        queue2.Enqueue(input_command);
                    }
                }
            }
        }

        static void f2()
        {
            int index,temp;
            while (work)
            {
                if (queue1.Count != 0)
                {
                    index = queue1.Dequeue();
                    Console.WriteLine("\t\tПоток №2 Работаю...");
                    for (int i = 0; i < work_emulation; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                        }
                    }
                    db[index - 1].print();
                }
            }
        }

        static void f3()
        {
            int index, temp;
            while (work)
            {
                if (queue2.Count != 0)
                {
                    Console.WriteLine("\t\t\tПоток №3 Работаю...");
                    for (int i = 0; i < work_emulation; i++)
                    {
                        for (int j = 0; j < 300; j++)
                        {
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                            temp = (i + i * i * i * i * i - i * 30) * i * i;
                        }
                    }
                    index = queue2.Dequeue();
                    db[index - 1].some_calculations();
                }
            }
        }


        static void Main(string[] args)
        {
            data d1 = new data(1, 234, 34, 23, 3456);
            db.Add(d1);
            data d2 = new data(2, 200, 35, 43, 2341);
            db.Add(d2);
            data d3 = new data(3, 264, 65, 34, 1234);
            db.Add(d3);
            data d4 = new data(4, 231, 32, 24, 3553);
            db.Add(d4);
            data d5 = new data(5, 634, 64, 53, 3450);
            db.Add(d5);
            data d6 = new data(6, 253, 14, 13, 1456);
            db.Add(d6);
            data d7 = new data(7, 634, 64, 63, 6456);
            db.Add(d7);

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Console.Read();
        }
    }
}
