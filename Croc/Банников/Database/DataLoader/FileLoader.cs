using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataLoader
{
    /// <summary>
    /// Загрузчик файлов
    /// </summary>
    class FileLoader
    {
        /// <summary>
        /// Соединение с базой данных
        /// </summary>
        private SqlConnection conn;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileLoader()
        {
            // Создание объекта соединения
            conn = new SqlConnection();
            // Строка соединения с БД
            conn.ConnectionString = "server=172.26.28.15;database=Student;user id=student;password=Student1";
            conn.Open();
        }

        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <param name="name">Имя файла</param>
        public void Load(string name)
        {
            // Прочитать весь файл
            string[] file = System.IO.File.ReadAllLines(name);

            // Разбор заголовка файла
            string[] header = file[0].Split(';');

            for (int n = 1; n < 100; n++) // file.Length
            {
                string sql = "INSERT INTO Bannikov (";
                // Формирование списка полей
                for (int i = 0; i <= 1; i++)
                {
                    if (i > 0)
                    {
                        sql += ",";
                    }
                    sql += "[" + header[i] + "]";
                }
                sql += ") VALUES (";
                string[] row = file[n].Split(';');
                // Формирование списка значений
                for (int i = 0; i <= 1; i++)
                {
                    if (i > 0)
                    {
                        sql += ",";
                    }
                    sql += "@" + header[i];
                }
                sql += ")";
                // Преобразование данных
                DateTime date, time;
                // Преобразование даты
                if (!DateTime.TryParseExact(row[0], "dd.MM.yyyy",
                    System.Globalization.CultureInfo.CurrentCulture,
                    System.Globalization.DateTimeStyles.None, out date))
                {
                    throw new ApplicationException("Ошибка при преобразовании даты");
                }
                // Преобразование времени
                if (!DateTime.TryParseExact(row[1].Substring(0, 8), "hh:mm:ss",
                    System.Globalization.CultureInfo.CurrentCulture,
                    System.Globalization.DateTimeStyles.None, out time))
                {
                    throw new ApplicationException("Ошибка при преобразовании времени");
                }
                // Исполнение SQL-запроса
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("DATE", date);
                cmd.Parameters.AddWithValue("TIME", time);
                cmd.ExecuteNonQuery();
                // 
                Console.Write(n + "\r");
            }
        }
    }
}
