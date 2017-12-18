using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("XMLFile1.xml");
            XmlNodeList nodeList;
            XmlNode root = doc.DocumentElement;
            nodeList = root.SelectNodes("Book");
            foreach (XmlNode book in nodeList)
                if (book.Attributes["Topic"].Value.Contains("XML"))
                    Console.WriteLine(book["Title"].InnerText);
            Console.ReadLine();

        }
    }
}