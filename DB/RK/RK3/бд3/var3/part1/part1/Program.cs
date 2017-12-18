using System;
using System.Xml;
using System.Globalization;

namespace part1
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
            {
                int count = 0;
                XmlNodeList author = book["Authors"].SelectNodes("Author");
                foreach (XmlNode a in author)
                    count++;
                if (count == 1)
                    Console.Write(book["Title"].InnerText + "\n");
            }
                    
            Console.ReadLine();

        }
    }
}
