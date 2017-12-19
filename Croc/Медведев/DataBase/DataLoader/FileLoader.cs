using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace DataLoader
{
    class FileLoader
    {
        /// <summary>
        /// соединение с бд
        /// </summary>
        private SqlConnection conn;



        /// <summary>
        /// constructor
        /// </summary>
        public FileLoader()
        {

            //conn = new SqlConnection();
            ////строка соединия с бд
            //conn.ConnectionString = "server=172.26.28.15; database=Student; user id= student; password=Student1";
            //conn.Open();

        }
        /// <summary>
        /// Загрузка пачки файлов по маске
        /// </summary>
        /// <param name="path">путь к файлу или группе файлов в стиле C:\FOLDER\*.CSV</param>
        public  void LoadPath(string path)
        {
            //слеш разделитель папок, в win и linux  разные слеши
            // System.IO.Path.DirectorySeparatorChar - платформонезависимый способ узнать это
            string sep = @"\";
            
            int lastsep = path.LastIndexOf(sep);//находим папку в которой мы собираемся что-то искать
            string targetDirectory = path.Substring(0, lastsep);
            Console.WriteLine("scanning folder {0}", targetDirectory);

            //шаблон регулярного выражения полученный из хвоста введенной строки
            string reg = path.Replace(targetDirectory + sep, "");
            Console.WriteLine(reg);

            //получаем список файлов в папке и идем по ним, в качестве маски могут быть только * и ?
            string[] fileEntries = Directory.GetFiles(targetDirectory, reg);
            foreach (var file in fileEntries)
            {
                Load(file);
            }
        }


        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <param name="name">Имя файла</param>
        public void Load(string name)
        {
            Console.WriteLine("File {0} loaded", name);
            string[] f = System.IO.File.ReadAllLines(name);
            foreach (string line in f)
            {
               // Console.WriteLine(line);
                    
            }
            return;
            ///код дальше не актуален без доступа к базе

            // прочитать весь файл
            string[] file = System.IO.File.ReadAllLines(name);
            string[] header = file[0].Split(';');

            for (int n=1; n<file.Length; n++)
            {
                string sql = "INSERT INTO Table_Medvedev (";

                for (int i = 0; i <= 2; i++)
                {
                    if (i > 0)
                    {
                        sql += ",";
                    }
                    sql += "[" + header[i]+"]" ;
                }
                
                sql+=") VALUES(";

                string[] row = file[n].Split(';');

                
                //for (int i = 0; i <= 2; i++)
                //{
                //    if (i > 0)
                //    {
                //        sql += ",";
                //    }
                //    sql += " " + row[i] + " ";
                //}
                //sql += ")";
                DateTime date, time;
                DateTime.TryParseExact(row[0], "dd.mm.yyyy", System.Globalization.CultureInfo.CurrentCulture,
                                                                                                         System.Globalization.DateTimeStyles.None, out date);

              
               
                DateTime.TryParseExact(row[1].Substring(0,0), "hh.mm.ss", System.Globalization.CultureInfo.CurrentCulture,
                                                                                                         System.Globalization.DateTimeStyles.None, out time);

                Console.WriteLine(date);
                Console.WriteLine(time);
                sql += date + "," + time+")";
                Console.WriteLine(sql);
                Console.ReadKey();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                Console.WriteLine();

            }
          
            
          


           

        }
    }
}
