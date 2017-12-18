using System;
using System.Collections.Generic;
using System.IO;


namespace DB.Generators
{
    public class BaseGenerator<T>
    {
        protected readonly Random Rnd = new Random();
        protected readonly List<T> Lst = new List<T>();

        public List<T> GetList => Lst;
        public void Printer()
        {
            foreach (var item in Lst)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public void ToFile(string path)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (var item in Lst)
                {
                    file.WriteLine(item.ToString());

                }
            }
        }



        protected DateTime GenDate()
        {
            int y = Rnd.Next(2015, 2018);
            int m = Rnd.Next(1, 13);

            int d = Rnd.Next(DateTime.DaysInMonth(y, m)) + 1;
            int h = Rnd.Next(24);
            int mm = Rnd.Next(60);
            int s = Rnd.Next(60);

            DateTime dt = new DateTime(y, m, d, h, mm, s);
            return dt;
        }

        protected long GenerateNdigitsN(int n)
        {
            long result = 0;
            for (int i = 0; i < n; i++)
            {
                result *= 10;
                result += Rnd.Next(9);

            }
            return result;

        }
    }
}
