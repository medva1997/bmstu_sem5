using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using  System.Configuration.Install;
using  System.Reflection;

namespace CROC.EDUACATION.CSharpBotService
{
    class Program
    {
        static void Main(string[] args)
        {
            //элемент расписания
            CSharpBotService.Storage.Schedule schedule = new Storage.Schedule()
            {
                Date = new DateTime(2017, 11, 7),
                TimeStart=new TimeSpan(18,0,0),
                TimeEnd = new TimeSpan(21,0,0),
                Topic="XAAAAA"
            };

            string cmd = (args.Count() > 0) ? args[0] : "";

            string s;
            try
            {

           
            switch (cmd)
                {
                case "console":
                    // на моем компе английская винда, и с русскими буквами проблемы
                    Console.OutputEncoding = Encoding.GetEncoding(1251);
                    CSharpBot bot = new CSharpBot();

                    //bot.AddLesson(schedule);
                    bot.Start();
                    Console.WriteLine("Using bot {0}", "@medva1997_home_bot");
                    Console.WriteLine("Скорее всего вам нужно заглянуть в APP.conf и поправить подключение к бд");
                    Console.WriteLine("В бд не плохо бы заполнить расписание занятий. Дата занятий должна быть в виде 2017-11-06 00:00:00.000");
                    //Как называется тип для даты без времени не понятно.
                    Console.ReadKey();
                    bot.Stop();
                    break;

                case "install":
                    s = Assembly.GetExecutingAssembly().Location;
                    
                    ManagedInstallerClass.InstallHelper(new string[]{s});
                    break;

                case "uninstall":
                    s = Assembly.GetExecutingAssembly().Location;
                    ManagedInstallerClass.InstallHelper(new string[] { "/u",s });
                    break;
                //C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe C:\TFS\Autumn\Медведев\Services\Services\CSharpBotService\bin\Debug\CSharpBotService.exe
                default:
                    var scr = new BotService();
                    ServiceBase.Run(scr);
                    break;
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
                
            }

        }
    }
}
