using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;

namespace ITBridgeService
{
    /// <summary>
    /// Для установки сервиса в операционной системе его надо запустить с параметром install:
    ///     ITBridgeService.exe install
    /// Для удаления сервиса из операционной системы его надо запустить с параметром delete:
    ///     ITBridgeService.exe delete
    /// Для запуска сервиса как простой программы его надо запустить с параметром console
    ///     ITBridgeService.exe console
    /// Запуск сервиса как сервиса осуществляется через панель управления или команду
    ///     net start ITBridgeService
    /// </summary>
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

                // Имя файла самого сервиса
                string s = Assembly.GetExecutingAssembly().Location;

                BridgeService svc;

                // Селектор команд
                switch (cmd)
                {
                    case "console": // Консольный запуск сервиса
                        svc = new BridgeService();
                        svc.Start();
                        Console.WriteLine("Сервис запущен, для завершения нажмите Enter");
                        Console.ReadLine();
                        svc.Stop();
                        break;

                    case "install":
                        ManagedInstallerClass.InstallHelper(new string[] { s });
                        Console.WriteLine("Для завершения нажмите Enter");
                        Console.ReadLine();
                        break;

                    case "uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", s });
                        Console.WriteLine("Для завершения нажмите Enter");
                        Console.ReadLine();
                        break;

                    default: // Запуск в виде сервиса
                        svc = new BridgeService();
                        ServiceBase.Run(svc);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Для завершения нажмите Enter");
                Console.ReadLine();
            }
        }
    }
}
