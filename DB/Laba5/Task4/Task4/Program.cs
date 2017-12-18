using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Task4
{
    class Program
    {
        /*Задание 4 
             *  Написать консольное приложение на языке C#, которое выполняет проверку допустимости
             *  разработанного в текущей ЛР XML-документа, используя XSD-схему.
             *  Проведите эксперименты с XML-документом и убедитесь в том, что приложение действительно
             *  обнаруживает ошибки при проверке допустимости.
             */
        private static bool flag;
        static void Main(string[] args)
        {
            flag = true;
            XmlSchemaCollection sc = new XmlSchemaCollection
            {
                { "", "example.xsd" }
            };
            XmlTextReader tr = new XmlTextReader("example.xml");
            XmlValidatingReader vr = new XmlValidatingReader(tr)
            {
                ValidationType = ValidationType.Schema
            };
            vr.ValidationEventHandler += new ValidationEventHandler(MyHandler);
            vr.Schemas.Add(sc);
            try
            {
                while (vr.Read())
                {
                    if (vr.NodeType == XmlNodeType.Element && vr.LocalName == "NumEmps")
                    {
                        int num = XmlConvert.ToInt32(vr.ReadElementString());
                        Console.WriteLine("Number of employees: " + num);
                    }
                }
            }
            catch (XmlException ex)
            {
                flag = false;
                Console.WriteLine("XMLException occurred: " + ex.Message);
            }
            finally
            {
                vr.Close();
            }

            if (flag == true)
            {
                Console.WriteLine("OK");
            }
            Console.ReadKey();

        }

        // Validation event handler method 
        public static void MyHandler(object sender, ValidationEventArgs e)
        {
            flag = false;
            Console.WriteLine("Validation Error: " + e.Message);
        }
    }
}
