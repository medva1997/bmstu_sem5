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
            string text = doc.ToString();
            var doc1 = XDocument.Parse(text);

            var Price = doc1.Root.Elements("Book").Select(c => (float)c.Element("Price")).ToArray();
            float minPrice = Price.Min();

            string mp = minPrice.ToString().Replace(",", ".");
                        
            IEnumerable < XElement > tests =
                    from el in root.Elements("Book")
                    where ((string)el.Element("Price")).Contains(mp)
                    select el;
            foreach (XElement el in tests)
            {
                Console.WriteLine((string)el.Element("Title"));
            }
            Console.ReadLine();
        }
    }
}
