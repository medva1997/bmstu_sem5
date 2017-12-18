using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace part3
{
    class Program
    {
        static void Main(string[] args)
        {
            XElement root = XElement.Load("XMLFile2.xml");
            XDocument doc = XDocument.Load("XMLFile2.xml");

            IEnumerable<XElement> all =
                    from el in root.Elements("course").Elements("instructor")
                    select el;


            IEnumerable<XElement> inst =
                (from el in all               
                select el).GroupBy(x => x.Value).Select(x => x.First());

            inst.Remove();

            foreach (XElement el in all)
                Console.Write(el.Value + "\n");


            Console.ReadLine();
        }
    }
}
