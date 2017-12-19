using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;

namespace CROC.Education.CSharpBotService
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
                // Первый аргумент командной строки
                string cmd = (args.Count() > 0) ? args[0] : "";

                // Файл самого сервиса
                string s = Assembly.GetExecutingAssembly().Location;

                // Селектор команд
                switch (cmd)
                {
                    case "console":
                        var bot = new CSharpBot();
                        bot.Start();
                        Console.WriteLine("Бот запущен, для завершения нажмите Enter");
                        Console.ReadLine();
                        break;

                    case "install":
                        ManagedInstallerClass.InstallHelper(new string[] { s });
                        Console.ReadLine();
                        break;

                    case "uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", s });
                        Console.ReadLine();
                        break;

                    default: // Запуск в виде сервиса
                        var svc = new BotService();
                        ServiceBase.Run(svc);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
