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
            XElement root = XElement.Load("XMLFile1.xml");
            XDocument doc = XDocument.Load("XMLFile1.xml");
            
            IEnumerable<XElement> tests =
                    from el in root.Elements("Book")
                    where el.Element("Authors").Descendants().Count() == 1
                    select el;
            
            foreach (XElement el in tests)
                Console.WriteLine((string)el.Element("Title"));
            Console.ReadLine();
        }
    }
}
