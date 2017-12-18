using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Medvedev
{
    class Program
    {
        static void Printer(dynamic res)
        {
            foreach (var type in res)
            {
                Console.WriteLine(type);
            }
            Console.WriteLine();
        }

      

        static void Main(string[] args)
        {
            
            Console.OutputEncoding= Encoding.UTF8;
            Console.WriteLine("Напечатать названия книг, написанных одним автором.");
            
            XElement root = XElement.Load("XMLFile1.xml");
            var tests =
                from el in root.Elements("Book")
                where el.Element("Authors").Descendants().Count() == 1
                select  new
                {
                    Title = el.Element("Title").Value
                  
                };

            Printer(tests);
            Console.ReadKey();

            

           
            Console.ReadKey();
        }
    }
}
