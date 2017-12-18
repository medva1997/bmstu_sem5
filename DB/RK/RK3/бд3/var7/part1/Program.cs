using System;
using System.Xml;
using System.Globalization;

// напечатать названия самых дешевых книг
namespace part1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("XMLFile1.xml");
            XmlNodeList bookList;
            XmlNode root = doc.DocumentElement;
            string cheapest_price;
            float min_price = 2000;
            bookList = root.SelectNodes("Book");
            foreach (XmlNode book in bookList)
            {
                float f;
                float.TryParse(book["Price"].InnerText, NumberStyles.Any, new CultureInfo("en-US"), out f);
                if (f < min_price)
                {
                    min_price = f;
                    cheapest_price = book["Price"].InnerText;
                }
            }

            foreach (XmlNode book in bookList)
            {
                if (book["Price"].InnerText.Contains(cheapest_price))
                {
                    Console.Write("Title: ");
                    Console.Write(book["Title"].InnerText);
                }
            }
            Console.Read();
        }
    }
}
