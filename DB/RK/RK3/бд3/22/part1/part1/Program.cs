using System;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;

namespace part1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("XMLFile2.xml");
            XmlNodeList nodeList;
            List<string> names = new List<string>(20);
            List<string> result = new List<string>(5);
            XmlNode root = doc.DocumentElement;
            nodeList = root.SelectNodes("course");
            foreach (XmlNode node in nodeList)
                names.Add(node["instructor"].InnerText);
            foreach (string name1 in names)
            {
                int count = 0;
                foreach (string name2 in names)
                {
                    if (name1 == name2)
                        count++;
                }
                if (count >= 2)
                {
                    if (result.IndexOf(name1) == -1)
                        result.Add(name1);
                }
            }
            foreach (string name in result)
                Console.Write(name + "\n");
            Console.ReadLine();
        }
    }
}
