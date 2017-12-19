using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WcfConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // var svc = new WcfService();
                var host = new ServiceHost(typeof (WcfService));
                host.Open();
                Console.WriteLine("Сервис запущен, для завершения нажмите Enter");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
