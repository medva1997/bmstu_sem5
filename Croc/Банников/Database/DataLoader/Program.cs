using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    class Program
    {
        /// <summary>
        /// Главная функция программы
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                FileLoader loader = new FileLoader();
                loader.Load(@"C:\TFS\Банников\Database\20131101.csv");
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
#if DEBUG
                // Подождать нажатие клавиши
                Console.ReadLine();
#endif
            }
        }
    }
}
