using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Medvedev_task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Напечатать названия книг, написанных одним автором.");

            XmlDocument doc = new XmlDocument();
            doc.Load("XMLFile1.xml");
            XmlNode root = doc.DocumentElement;
            XmlNodeList nodeList = root.SelectNodes("Book");

            foreach (XmlNode book in nodeList)
            {
                int count = 0;
                XmlNodeList author = book["Authors"].SelectNodes("Author");
                foreach (XmlNode a in author)
                    count++;
                if (count == 1)
                    Console.Write(book["Title"].InnerText + "\n");
            }

            Console.ReadKey();
        }
    }
}
