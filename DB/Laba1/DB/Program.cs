using System;
using DB.Generators;

namespace DB
{
    class Program
    {
        static void Main(string[] args)
        {
            int nVisitors = 1000;
            int nManagers = 100;
            int nCompanies = 20;

            string path = "C:\\Users\\medva\\source\\repos\\Labs_DB\\DB\\inDATA\\";
            int nMettings = nManagers * 7;

            string outFolder = "C:\\Users\\medva\\source\\repos\\Labs_DB\\DB\\outDATA\\";

            WorkHours wh = new WorkHours()
            {
                EventStart = new DateTime(2017, 11, 1),
                EventEnd = new DateTime(2017, 11, 5),
                DayStart = new TimeSpan(8, 30, 00),
                DayEnd = new TimeSpan(20, 00, 00),
                Step = new TimeSpan(0, 15, 0)
            };

            BusyGenerator genBusyVisitors = new BusyGenerator(wh, nVisitors);

            BusyGenerator genBusyManagers = new BusyGenerator(wh, nManagers);

             //genBusyVisitors.Printer();
             //genBusyManagers.Printer();

            VisitorGenerator visitors= new VisitorGenerator(nVisitors,path+ @"FIO\\");
            ManagerGenerator managers =new ManagerGenerator(nManagers,path+ @"FIO\\", nCompanies);


            MeetingGenerator meeting= new MeetingGenerator(nManagers,nVisitors,nMettings,wh, genBusyManagers, genBusyVisitors);
            CompGenerator companies= new CompGenerator(path,nCompanies);


            visitors.ToFile(outFolder + "visitors.txt");
            managers.ToFile(outFolder + "managers.txt");
            genBusyManagers.ToFile(outFolder + "busy_managers.txt");
            genBusyVisitors.ToFile(outFolder + "busy_visitors.txt");
            meeting.ToFile(outFolder + "meetings.txt");
            companies.ToFile(outFolder + "comp.txt");

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
