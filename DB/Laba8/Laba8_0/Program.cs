using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Data.Linq;


namespace Laba8_0
{

    public class Compa
    {
        public int id;
        public string name;
    }
    public class Manager2
    {

        public int Mid;
        public int CompId;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName;
        /// <summary>
        /// Отчество
        /// </summary>
        public string SecondName;
        /// <summary>
        /// Почта
        /// </summary>
        public string Email;
        /// <summary>
        /// Пароль	для	входа
        /// </summary>
        public string Password;
        /// <summary>
        /// Номер	телефона
        /// </summary>
        public string PhoneNumber;

        public override string ToString()
        {
            string sep = " ";
            string line = "";
            line += Mid + sep;
            line += CompId + sep;
            line += LastName + sep;
            line += FirstName + sep;
            line += SecondName + sep;
            line += Email + sep;
            line += Password + sep;
            line += PhoneNumber;
            return line;
        }
    }
    class Program
    {
        static void Printer(dynamic res)
        {
            foreach (var type in res)
            {
                Console.WriteLine(type);
            }
            Console.WriteLine();
        }


        public static  XmlDocument GetXML()
        {

            //SqlConnection connection = new SqlConnection("server=127.0.0.1;database=MYWATCH2; user id=AAA; password=AAA");
            //connection.Open();
            //SqlCommand command = new SqlCommand();
            //command.CommandText = "SELECT * from Manager2 FOR XML AUTO";
            //command.Connection = connection;

            //XmlReader reader = command.ExecuteXmlReader();
            //XmlDocument myDocument= new XmlDocument();

            //myDocument.Load(reader);
            //connection.Close();
            XmlDocument myDocument = new XmlDocument();
            FileStream myFile = null;
            try
            {
                myFile = new FileStream("ex2.xml", FileMode.Open);
                XmlValidatingReader myReader = new XmlValidatingReader(myFile, XmlNodeType.Element, null);
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

            return myDocument;
        }

        static void Main(string[] args)
        {
            List<Compa> companies = new List<Compa>() { new Compa(){name = "Первая", id = 1},
                new Compa(){name = "Вторая", id = 2},new Compa(){name = "Третья", id = 3},
                new Compa(){name = "Четвертая", id = 4},new Compa() {name = "Пятая", id = 5}};

            Console.OutputEncoding = Encoding.UTF8;
            ManagerGenerator mg = new ManagerGenerator(50, 5);

            List<Manager2> lst = mg.GetList;

            Console.WriteLine("Список людей из 5 компании");
            var res1 = from manager in lst
                       where manager.CompId == 5
                       select new { manager.FirstName, manager.LastName, manager.CompId };

            Printer(res1);

            Console.WriteLine("Количество сотрудников в компании");
            var res2 = from manager in lst
                       group manager by manager.CompId into g
                       orderby g.Key ascending
                       select new { Name = g.Key, Count = g.Count() };

            Printer(res2);

            Console.WriteLine("Количество сотрудников отсортированное по количеству в сторону уменьшения");
            var res3 = from manager in lst
                       group manager by manager.CompId
                into g
                       orderby g.Count() descending
                       select new { Name = g.Key, Count = g.Count() };

            Printer(res3);

            Console.WriteLine("Количество сотрудников в компании отсортированное по количеству в сторону возрастания");
            var res4 = from manager in lst
                       group manager by manager.CompId into g
                       orderby g.Count() ascending
                       select new { Name = g.Key, Count = g.Count() };

            Printer(res4);


            Console.WriteLine("Фамилия сотрудника и название его комании");
            var res5 = from manager in lst
                       join comp in companies on manager.CompId equals comp.id
                       where manager.Mid <= 10
                       select new { name = manager.LastName, CompName = comp.name };

            Printer(res5);
            Console.WriteLine("Фамилия сотрудника и название его комании если длинна почтового адреса> 22");
            var res6 = from manager in lst
                let M = manager.Email
                join comp in companies on manager.CompId equals comp.id
                where manager.Mid <= 10 && M.Length > 22
                select new { name = manager.LastName, CompName = comp.name, Email= M };

            Printer(res6);
          

            



            Console.WriteLine("press any key");
            Console.ReadKey();

            //XmlDocument xml = GetXML();
            //Console.WriteLine(xml);


            XDocument xdoc = XDocument.Load("ex2.xml");

            Console.WriteLine("Вывод из xml работников 2 компании");
            var items = from xe in xdoc.Elements("Managers").Elements("Manager2")
                        where xe.Element("CompID").Value=="2"
                select new {
                    Name = xe.Element("LastName").Value,
                    FName = xe.Element("FirstName").Value
                };
            Printer(items);


            xdoc = XDocument.Load("ex2.xml");
            XElement root = xdoc.Element("Managers");

            foreach (XElement xe in root.Elements("Manager").ToList())
            {
              
                if (xe.Element("CompID").Value == "2")
                {
                    xe.Element("CompID").Value = "2000";
                }
                if (xe.Element("MID").Value == "1")
                {
                    xe.Remove();
                }
            }

            root.Add(new XElement("Manager2",
                new XAttribute("MID", "11000"),
                new XElement("LastName", "Qwerty")));
            xdoc.Save("ex3.xml");

            Console.WriteLine("Xml изменен и сохранен");

            string connectionString = "server=127.0.0.1;database=MYWATCH2; user id=AAA; password=AAA";
            DataContext db = new DataContext(connectionString);

            // Получаем таблицу пользователей
            //Table<Visitor> users = db.GetTable<Visitor>();
            //Однотабличный запрос на выборку.
            Console.WriteLine("Первые 10 посетителей");
            var users = from u in db.GetTable<Visitor>()
                where u.VID < 11
                orderby u.LastName
                select u;
            foreach (var user in users)
            {
                Console.WriteLine("{0} \t\t\t{1} \t\t\t{2}", user.LastName, user.FirstName, user.VID);
            }

            //Многотабличный запрос на выборку.
            var comps = from c in db.GetTable<Company>()
                join manager1 in db.GetTable<Manager>() on c.CompAdminMID equals manager1.MID
                select new {Name = c.CompName, Fname = manager1.LastName};
            Printer(comps);


            
            var userdb= db.GetTable<Visitor>().Where(c=>c.VID==1008).FirstOrDefault();
            Console.WriteLine("Старая фамилия "+userdb.LastName);
            userdb.LastName = userdb.LastName + 1;
            Console.WriteLine("Новая фамилия " + userdb.LastName);
            Console.WriteLine("Save");
            db.SubmitChanges();
            var userdb1 = db.GetTable<Visitor>().Where(c => c.VID == 1008).FirstOrDefault();
            Console.WriteLine("NOW " + userdb1.LastName);


            Console.WriteLine("Вставка 1009");
            Visitor user1 = new Visitor()
            {
                VID = 1009,
                FirstName = "Алексей",
                LastName = "Медведев",
                SecondName = "Вячеславович",
                Email = "web@medalexey.ru",
                Password = "CCB7D615747225F0442C97BAFCFD5700",
                PhoneNumber = "79262300359"
            };
            db.GetTable<Visitor>().InsertOnSubmit(user1);
            db.SubmitChanges();
            Console.WriteLine("Вставка завершена");
            Console.WriteLine("Нажмите кнопку для удаления этого элемента");
            Console.ReadKey();

            var user2 = db.GetTable<Visitor>().Where(c => c.VID == 1009).FirstOrDefault();
            db.GetTable<Visitor>().DeleteOnSubmit(user2);
            db.SubmitChanges();
            Console.WriteLine("Объект удален");

            UserDataContext.UserDataContext1 db1 = new UserDataContext.UserDataContext1(connectionString);
            int min = 0, max = 0;
          
            db1.GetIndexRange(ref min, ref max);
            Console.WriteLine($"{max} {min}");
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
