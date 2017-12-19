using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FileLoader loader = new FileLoader();
                //Console.WriteLine("DB OK");
                //loader.Load(@"C:\TFS\Autumn\Медведев\DataBase\20131101.csv");
                Console.WriteLine("FILE OK");
                loader.LoadPath(@"C:\TFS\Банников\Database\*.csv");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
#if DEBUG
                // подождать клаву
                Console.ReadLine();
#endif          

            }
        }
    }
}
