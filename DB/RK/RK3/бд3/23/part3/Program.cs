using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            XElement root = XElement.Load("XMLFile1.xml");
            IEnumerable<XElement> tests =
                from el in root.Elements("Book")
                where ((string)el.Attribute("Topic")).Contains("XML")
                select el;
            foreach (XElement el in tests)
                Console.WriteLine((string)el.Element("Title"));
            Console.ReadLine();

        }
    }
}
