using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace lab6
{
    class Worker
    {
        private XmlDocument _myDocument;

        private XmlDocument myDocument
        {
            get
            {
                if (_myDocument == null)
                {
                    Console.WriteLine("Файл не был загружен, выполняется загрузка");
                    LoadFile();
                }
                return _myDocument;
            }
            set { _myDocument=value; }
        }
        /// <summary>
        /// Открытие документа, находящегося в файле.
        /// </summary>
        private void LoadFile()
        {
            myDocument = new XmlDocument();
            FileStream myFile=null;
            try
            {
                myFile = new FileStream("example.xml", FileMode.Open);
                XmlValidatingReader myReader = new XmlValidatingReader(myFile, XmlNodeType.Document, null);
                myDocument.Load(myReader);
               
                Console.WriteLine("Файл загружен");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            finally
            {
                if (myFile != null)
                {
                    myFile.Close();
                }
            }
           
            MainMenu();
        }

        /// <summary>
        /// Поиск информации, содержащейся в документе.
        /// </summary>
        private void Search()
        {
            Console.WriteLine("Поиск информации, содержащейся в документе:");
            Console.WriteLine("1. с помощью метода GetElementsByTagName");
            Console.WriteLine("2. с помощью метода GetElementsById");
            Console.WriteLine("3. с помощью метода SelectNodes");
            Console.WriteLine("4. с помощью метода SelectSingleNode");
            Console.WriteLine("5. Вернуться в главное меню");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int option) && option >= 1 && option <= 5)
            {
                switch (option)
                {
                    case 1: GetByTag(); break;
                    case 2: GetByID(); break;
                    case 3: GetByNode(); break;
                    case 4: GetBySingleNode(); break;
                    case 5: MainMenu(); break;
                }
            }
            else
            {
                Console.WriteLine("Что-то не так. Попробуйте выбрать еще раз");
                Search();
            }
        }

        /// <summary>
        /// Поиск информации, содержащейся в документе с помощью метода GetElementsByTagName
        /// </summary>
        private void GetByTag()
        {
            Console.Write("Введите тег:");
            string tag= Console.ReadLine();
            if (tag != null)
            {
                XmlNodeList taglist = myDocument.GetElementsByTagName(tag);
                if (taglist.Count == 0)
                {
                    Console.WriteLine("Ничего не найдено");
                }
                foreach (XmlNode element in taglist)
                {
                    Console.WriteLine(element.ChildNodes[0].Value);
                }
            }
            else
            {
                Console.WriteLine("Плохой тег");
            }
            MainMenu();
        }

        /// <summary>
        /// Поиск информации, содержащейся в документе с помощью метода GetElementsById
        /// </summary>
        private void GetByID()
        {
           
            Console.Write("Введите ID:");
            string tag = Console.ReadLine();
            if (tag != null)
            {
                XmlElement element= myDocument.GetElementById(tag);
                if (element == null)
                {
                    Console.WriteLine("Ничего не найдено");
                }
                else
                {
                    Console.WriteLine(element.ChildNodes[0].ChildNodes[0].Value);
                }
            }
            else
            {
                Console.WriteLine("Плохой ID");
            }
            MainMenu();
        }

        /// <summary>
        /// Поиск информации, содержащейся в документе с помощью метода SelectNodes
        /// </summary>
        private void GetByNode()
        {
          
            Console.WriteLine("Время начала встречи в комнате 3");
            //linConsole.ReadLine()
            XmlNodeList Node = myDocument.SelectNodes("//Meeting[Location='Room 3']");
            for (int i = 0; i < Node.Count; i++)
                Console.Write(Node[i].ChildNodes[0].ChildNodes[0].InnerText+ "\r\n");
            MainMenu();
        }

        /// <summary>
        /// Поиск информации, содержащейся в документе с помощью метода SelectSingleNode
        /// </summary>
        private void GetBySingleNode()
        {
            //Console.WriteLine("Время начала встречи в комнате 3");
            Console.WriteLine("Введите номер комнаты");
            string line = Console.ReadLine();
            int.TryParse(line, out int n);

            XmlNode Node = myDocument.SelectSingleNode($"//Meeting[Location='Room {n}']");
            Console.Write(Node.ChildNodes[0].ChildNodes[0].InnerText + "\r\n");
            MainMenu();
        }

        /// <summary>
        /// Доступ к содержимому узлов
        /// </summary>
        private void NodeUsage()
        {
            Console.WriteLine("Доступ к содержимому узлов:");
            Console.WriteLine("1. к узлам типа XmlElement");
            Console.WriteLine("2. к узлам типа XmlТext");
            Console.WriteLine("3. к узлам типа XmlComment");
            Console.WriteLine("4. к узлам типа XmlProcessingInstruction");
            Console.WriteLine("5. к атрибутам узлов");
            Console.WriteLine("6. Вернуться в главное меню");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int option) && option >= 1 && option <= 6)
            {
                switch (option)
                {
                    case 1: AccessElement(); break;
                    case 2: AccessText(); break;
                    case 3: AccessComment(); break;
                    case 4: AccessInstruction(); break;
                    case 5: AccessAtr(); break;
                    case 6: MainMenu(); break;
                }
            }
            else
            {
                Console.WriteLine("Что-то не так. Попробуйте выбрать еще раз");
                Search();
            }
        }

        /// <summary>
        /// Доступ к содержимому узлов к узлам типа XmlElement
        /// </summary>
        private void AccessElement()
        {
            Console.WriteLine("Getting <MID>");
            XmlElement cityElement = (XmlElement)myDocument.DocumentElement.ChildNodes[2];
            Console.Write(cityElement.ChildNodes[3].ChildNodes[0].ChildNodes[0].Value + "\r\n");
            MainMenu();
        }

        /// <summary>
        /// Доступ к содержимому узлов к узлам типа XmlТext
        /// </summary>
        private void AccessText()
        {
            Console.Write(myDocument.DocumentElement.ChildNodes[4].InnerText + "\r\n");
            Console.Write(myDocument.DocumentElement.ChildNodes[5].ChildNodes[1].Value+ "\r\n");
            MainMenu();
        }

        /// <summary>
        /// Доступ к содержимому узлов к узлам типа XmlComment
        /// </summary>
        private void AccessComment()
        {
            Console.Write(myDocument.DocumentElement.ChildNodes[3].ChildNodes[1].Value + "\r\n");
            MainMenu();
        }

        /// <summary>
        /// Доступ к содержимому узлов к узлам типа XmlProcessingInstruction
        /// </summary>
        private void AccessInstruction()
        {
            XmlProcessingInstruction myPI = (XmlProcessingInstruction)
                myDocument.DocumentElement.ChildNodes[6].ChildNodes[1];
            Console.Write("Name: " + myPI.Name + "\r\n");
            Console.Write("Data: " + myPI.Data + "\r\n");
            MainMenu();
        }

        /// <summary>
        /// Доступ к содержимому узлов к атрибутам узлов
        /// </summary>
        private void AccessAtr()
        {
            Console.WriteLine("Атрибуты первого уровня");
           
            Console.Write("The meet attribute is: " + myDocument.DocumentElement.GetAttribute("Meet_ID"));

            foreach (XmlNode node in myDocument.ChildNodes)
            {
                foreach (XmlNode nodeChildNode in node.ChildNodes)
                {
                    XmlAttributeCollection myAttributes1 = nodeChildNode.Attributes;
                    if (myAttributes1 != null)
                    {
                        foreach (XmlAttribute atr in myAttributes1)
                        {
                            Console.Write("Attribute: " + atr.Name + " = " + atr.Value + "\r\n");
                        }
                    }
                }
            }
            MainMenu();
        }



        /// <summary>
        /// Внесение изменений в документ.
        /// </summary>
        private void Change()
        {
            Console.WriteLine("Внесение изменений в документ:");
            Console.WriteLine("1. удаление содержимого");
            Console.WriteLine("2. внесение изменений в содержимое");
            Console.WriteLine("3. создание нового содержимого");
            Console.WriteLine("4. вставка содержимого");
            Console.WriteLine("5. добавление атрибутов");
            Console.WriteLine("6. Выход в главное меню");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int option) && option >= 1 && option <= 6)
            {
                switch (option)
                {
                    case 1: ChangeRemove(); break;
                    case 2: ChangeChange(); break;
                    case 3: ChangeNew(); break;
                    case 4: ChangeInsert(); break;
                    case 5: ChangeAddAtr(); break;
                    case 6: MainMenu(); break;
                }
            }
            else
            {
                Console.WriteLine("Что-то не так. Попробуйте выбрать еще раз");
                Search();
            }
        }
        /// <summary>
        /// Внесение изменений в документ: удаление содержимого
        /// </summary>
        private void ChangeRemove()
        {
            Console.WriteLine("Удаление InstructionCode");
            XmlElement pcElement = (XmlElement)myDocument.GetElementsByTagName("InstructionCode")[0];
            myDocument.DocumentElement.RemoveChild(pcElement);
            Saver();
            MainMenu();
        }

        /// <summary>
        /// Внесение изменений в документ: внесение изменений в содержимое
        /// </summary>
        private void ChangeChange()
        {
            Console.WriteLine("Отсечение мс у TimeStart");
            XmlNodeList times = myDocument.SelectNodes("//Meeting/TimeStart/text()");
            for (int i = 0; i < times.Count; i++)
                times[i].Value = times[i].Value.Split('.')[0];
            Saver();
            MainMenu();
        }

        /// <summary>
        /// Внесение изменений в документ: создание нового содержимого
        /// </summary>
        private void ChangeNew()
        {
            Console.WriteLine("Вставка цвета в конец");
            XmlElement colorElement = myDocument.CreateElement("color");
            XmlText colorText = myDocument.CreateTextNode("red");
            colorElement.AppendChild(colorText);
            myDocument.DocumentElement.AppendChild(colorElement);
            Saver();
            MainMenu();
        }


        /// <summary>
        /// Внесение изменений в документ: вставка содержимого
        /// </summary>
        private void ChangeInsert()
        {
            Console.WriteLine("Вставка содержимого в manager с mid 36");
            Console.Write("Введите название тега:");
            string tag=Console.ReadLine();

            Console.Write("Введите содержимое тега:");
            string text = Console.ReadLine();

            XmlElement colorElement = myDocument.CreateElement(tag);
            XmlText colorText = myDocument.CreateTextNode(text);
            colorElement.AppendChild(colorText);
            myDocument.DocumentElement.FirstChild.ChildNodes[3].AppendChild(colorElement);
            Saver();
            MainMenu();
        }

        /// <summary>
        /// Внесение изменений в документ: добавление атрибутов
        /// </summary>
        private void ChangeAddAtr()
        {
            myDocument.DocumentElement.SetAttribute("something", "p17");
            Saver();
            MainMenu();
        }

        private void Saver()
        {
            Directory.GetFiles(System.IO.Directory.GetCurrentDirectory());
            
            var files= Directory.GetFiles(Directory.GetCurrentDirectory(), "example*.xml");
            var last = files.Max(file => file);
            last = last.Replace("example", "");
            last = last.Replace(".xml", "");
            last = last.Replace(Directory.GetCurrentDirectory()+"\\", "");
            int n = 0;
            int.TryParse(last, out n);
            n += 1;
            string filename = $"example{n}.xml";
            myDocument.Save(filename);
            Console.WriteLine($"Файл {filename} сохранен");

        }


        /// <summary>
        /// Главное меню
        /// </summary>
        private void MainMenu()
        {
            Console.WriteLine("1. Открытие документа, находящегося в файле.");
            Console.WriteLine("2. Поиск информации, содержащейся в документе.");
            Console.WriteLine("3. Доступ к содержимому узлов");
            Console.WriteLine("4. Внесение изменений в документ.");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int option) && option>=1 && option<=4)
            {
                switch (option)
                {
                    case 1: LoadFile();break;
                    case 2: Search();break;
                    case 3: NodeUsage();break;
                    case 4: Change(); break;
                }
            }
            else
            {
                Console.WriteLine("Что-то не так. Попробуйте выбрать еще раз");
                MainMenu();
            }
        }

        public Worker()
        {
            MainMenu();
        }
    }
}
