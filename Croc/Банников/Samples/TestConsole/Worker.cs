using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class Worker
    {
        /// <summary>
        /// Деление двух чисел
        /// </summary>
        /// <param name="a">Делитель</param>
        /// <param name="b">Делимое</param>
        /// <returns></returns>
        static public int Division(int a, int b)
        {
            return a / b;
        }

        /// <summary>
        /// Корень квадратный
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static public double Sqrt(double x)
        {
            return (x >= 0) ? Math.Sqrt(x) : 0;
        }

        /// <summary>
        /// Можно ли из букв длинного слова составить короткое
        /// </summary>
        /// <param name="shortWord">Короткое слово</param>
        /// <param name="longWord">Длинное слово</param>
        /// <returns></returns>
        static public bool CouldBuild(string shortWord, string longWord)
        {
            if (shortWord == null) return true;
            if (longWord == null) return false;
            var l = longWord.ToList<char>();
            foreach (char c in shortWord)
            {
                if (l.Exists(a => a == c))
                {
                    l.Remove(c);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
