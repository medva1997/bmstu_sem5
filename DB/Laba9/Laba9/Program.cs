using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Laba9
{
    class Program
    {
        private static string connectionString = "server=127.0.0.1;database=MYWATCH2; user id=AAA; password=AAA";

        /// <summary>
        /// Информация о подключении к базе
        /// </summary>
        static void Task1()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение>
                conn.Open();
                Console.WriteLine("Подключение открыто.");
                // Выводим на консоль свойства подключения
                Console.WriteLine("Свойства подключения:");
                Console.WriteLine("\tСтрока подключения: {0}", conn.ConnectionString);
                Console.WriteLine("\tБаза данных: {0}", conn.Database);
                Console.WriteLine("\tИсточник данных: {0}", conn.DataSource);
                Console.WriteLine("\tВерсия сервера: {0}", conn.ServerVersion);
                Console.WriteLine("\tСостояние подключения: {0}", conn.State);
            }
            catch (SqlException e)
            {
                // Выводим сообщение об ошибке в случае возникновения исключения
                Console.WriteLine("Ошибка: " + e);
            }
            finally
            {
                // Закрываем подключение
                conn.Close();
                Console.WriteLine("Подключение закрыто.");
            }
        }

        /// <summary>
        /// Скалярный запрос выводящий количесво поситителей
        /// </summary>
        static void Task2()
        {
            string queryString = @"select count(*) from Visitor";
            
            SqlConnection conn = new SqlConnection(connectionString);
            // Создаем команду для текущей строки запроса и текущего подключения
            SqlCommand cmd = new SqlCommand(queryString, conn);
            try
            {
                conn.Open();
                // Выполняем  скалярный запрос
                Console.WriteLine("Количество поситителей равно {0}", cmd.ExecuteScalar());
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// Список поситителей >1000
        /// </summary>
        static void Task3()
        {
            
            string queryString = @"select Vid, LastName, FirstName, SecondName from Visitor Where VID>1000";
           
            SqlConnection conn = new SqlConnection(connectionString);
            // Создаем команду для текущей строки запроса и текущего подключения
            SqlCommand cmd = new SqlCommand(queryString, conn);
           
            try
            {
                
                conn.Open();
                // Выполняем запрос и строим объект rdr типа SqlDataReader, 
                // который предоставляет возможность чтения потока строк только в прямом направлении из базы данных SQL Server
                SqlDataReader rdr = cmd.ExecuteReader();
                // Читаем строки в прямом направлении до тех пор, пока они есть
                while (rdr.Read())
                {
                    // Выводим на консоль интересующие нас данные
                    Console.WriteLine("ФИО поситителей: {0} {1} {2} {3} ", rdr.GetValue(0), rdr.GetValue(1), rdr.GetValue(2), rdr.GetValue(3)
                    );
                }
                // Закрываем читатель
                rdr.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                // Закрываем подключение
                conn.Close();
            }
        }

        /// <summary>
        /// Добавляем поситителя
        /// </summary>
        static void Task4()
        {

           
            string FirstName = "Алексей";
            Console.WriteLine("Введите фамилию");
            string LastName = Console.ReadLine();
            string SecondName = "Вячеславович";
            SqlConnection conn = new SqlConnection(connectionString);
            // Определяем скалярный запрос на выборку
            string sqlqry = @"select max(VID) from Visitor";
            // Определяем параметрический запрос на вставку
            string sqlins = @"insert into Visitor (VID, FirstName,LastName,SecondName) values(@vid,@fname, @lname,@sname)";
           
            // Создаем две команды
            SqlCommand cmdqry = new SqlCommand(sqlqry, conn);
            SqlCommand cmdnon = new SqlCommand(sqlins, conn);
            // Добавляем параметры к команде на вставку
            cmdnon.Parameters.Add("@fname", SqlDbType.NVarChar, 60);
            cmdnon.Parameters.Add("@lname", SqlDbType.NVarChar, 60);
            cmdnon.Parameters.Add("@sname", SqlDbType.NVarChar, 60);
            cmdnon.Parameters.Add("@vid", SqlDbType.Int);
            try
            {
                // Открываем подключение
                conn.Open();
                // Выполняем запрос на выборку, чтобы получить количество сотрудников до вставки
                int tmp = Convert.ToInt32(cmdqry.ExecuteScalar());
                Console.WriteLine("До INSERT:");
                Task2();
                // Присваиваем параметрам значения
                cmdnon.Parameters["@fname"].Value = FirstName;
                cmdnon.Parameters["@sname"].Value = SecondName;
                cmdnon.Parameters["@lname"].Value = LastName;
                cmdnon.Parameters["@vid"].Value = tmp+1;
                
                // Выполняем запрос на вставку
                cmdnon.ExecuteNonQuery();

               
                Console.WriteLine("После INSERT:");
                Task2();
                Task3();

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                // Закрываем подключение
                conn.Close();
                Console.WriteLine("Подключение закрыто.");
            }
        }

        /// <summary>
        /// Вызываем процедуру выводящюю список функций
        /// </summary>
        static void Task5()
        {
           
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                conn.Open();
                
                SqlCommand cmd = conn.CreateCommand();
                // Определяем хранимую процедуру для исполнения
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "FincInformation3";
                // Выполняем команду
                SqlDataReader rdr = cmd.ExecuteReader();
                // Обрабатываем результирующий набор данных
                while (rdr.Read())
                {
                    Console.WriteLine("{0}", rdr[0].ToString());
                }
                // Закрываем читатель
                rdr.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                
                conn.Close();
            }
            
        }

        /// <summary>
        /// Встречи манеджера с № введенным с клавиатуры 
        /// </summary>
        static void Task6()
        {
            // Создаем подключение
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                conn.Open();
                // Создаем команду
                SqlCommand cmd = conn.CreateCommand();
                // Определяем хранимую процедуру для исполнения
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.MeetingsByManger5";
                // Создаем входной параметр
                SqlParameter inparm = cmd.Parameters.Add("@MID", SqlDbType.Int);
                inparm.Direction = ParameterDirection.Input;
                Console.Write("Id менеджера:");
                if (int.TryParse(Console.ReadLine(),out  int id)==false)
                {
                    return;
                }
                inparm.Value = id;
                // Создаем выходной параметр
                SqlParameter ouparm = cmd.Parameters.Add("@meetcount", SqlDbType.Int);
                ouparm.Direction = ParameterDirection.Output;
                // Создаем параметр возвращаемого значения
                SqlParameter retval = cmd.Parameters.Add("return_value", SqlDbType.Int);
                retval.Direction = ParameterDirection.ReturnValue;
                // Выполняем команду, т.е. хранимую процедуру
                SqlDataReader rdr = cmd.ExecuteReader();
                // Обрабатываем результирующий набор данных
                while (rdr.Read())
                {
                    Console.WriteLine("{0} {1}", rdr[0].ToString(), rdr[1].ToString());
                }
                // Закрываем читатель
                rdr.Close();
                // Печатаем значение выходного параметра
                Console.WriteLine("Значение выходного параметра равно {0}", cmd.Parameters["@meetcount"].Value);
                // Печатаем возвращаемое значение
                Console.WriteLine("Возвращаемое значение равно {0}", cmd.Parameters["return_value"].Value);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Печатаеть поситителей с vid меньше 10
        /// </summary>
        static void Task7()
        {
            string sql = @"
                select *
                from Visitor
                where VID < 10
            ";
            // Создаем подключение
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                conn.Open();
                // Создаем адаптер данных
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                // Создаем объект ds типа DataSet
                DataSet ds = new DataSet();
                // Наполняем объект ds
                da.Fill(ds, "Visitor");
                // Получаем таблицу данных
                DataTable dt = ds.Tables["Visitor"];
                // Распечатываем содержимое таблтцы
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                        Console.WriteLine(row[col]);
                    Console.WriteLine("".PadLeft(30, '='));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e);
            }
            finally
            {
                // Закрываем подключение
                conn.Close();
            }
          
        }


        /// <summary>
        /// Фильтрация и сортировка по компании CompID б 5 And CompID > 1
        /// </summary>
        static void Task8()
        {
            string sql = @"select * from Manager ";
            
            // Создаем подключение
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // Создаем адаптер данных
                SqlDataAdapter da = new SqlDataAdapter();
                // Создаем команду и присваиваем ее свойству SelectCommand адаптера данных.
                da.SelectCommand = new SqlCommand(sql, conn);
                // Создаем объект ds типа DataSet и наполняем его данными
                DataSet ds = new DataSet();
                da.Fill(ds, "Manager");
                // Получаем коллекцию таблиц
                DataTableCollection dtc = ds.Tables;
               
                
                // Устанавливаем фильтр
                string fl = "CompID < 5 And CompID > 1";
                // Определяем порядок сортировки
                string srt = "CompID asc";
                // Выводим на консоль отфильтрованные и отсортированные данные
                foreach (DataRow row in dtc["Manager"].Select(fl, srt))
                {
                    Console.WriteLine("{0}\t{1}", row["CompId"].ToString().PadRight(25), row["LastName"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e);
            }
            finally
            {
                // Закрываем подключение
                conn.Close();
            }
        }

        /// <summary>
        /// Обновляем номер компании у первого сотрудника из комании №3
        /// </summary>
        static void Task9()
        {

            // Создаем  строку запроса на выборк
            string qry = @"select * from Manager where CompID = 3";
            // Создаем строку запроса на обновление
            string upd = @"update Manager set CompID = @CompID where MID = @MID";
            // Создаем подключение
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // Создаем адаптер данных
                SqlDataAdapter da = new SqlDataAdapter();
                // Создаем команду запроса для текущего подключения
                da.SelectCommand = new SqlCommand(qry, conn);
                // Создаем и наполняем таблицу набора данных
                DataSet ds = new DataSet();
                da.Fill(ds, "Manager");
                // Получаем ссылку на таблицу 
                DataTable dt = ds.Tables["Manager"];
                // Изменяем поле  первой записи
                dt.Rows[0]["CompID"] = 4;
               
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine("{0} {1} {2}",
                       row["LastName"].ToString().PadRight(15),
                       row["MID"].ToString().PadLeft(25),
                       row["CompID"]);
                }
                // Изменяем поле  первой записи
                
                // Обновляем таблицу  БД
                // Создаем команду обновления для текущего подключения
                SqlCommand cmd = new SqlCommand(upd, conn);
                cmd.Parameters.Add("@CompID", SqlDbType.Int, 4, "CompID");
                SqlParameter parm = cmd.Parameters.Add("@MID", SqlDbType.Int, 4, "MID"); ;
                parm.SourceVersion = DataRowVersion.Original;
                // Адаптер выполняет команду обновления, обнаруженную в свойстве 'UpdateCommand'
                da.UpdateCommand = cmd;
                da.Update(ds, "Manager");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            finally
            {
                // Закрываем подключение
                conn.Close();
            }
            
        }

        /// <summary>
        /// Выгрузка Visitor в XML
        /// </summary>
        static void Task10()
        {
           
            // Создаем строку запроса
            string qry = @"select * from Visitor";
            // Создаем подключение
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // Создаем адаптер данных
                SqlDataAdapter da = new SqlDataAdapter {SelectCommand = new SqlCommand(qry, conn)};
                // Открываем подключение
                conn.Open();
                // Создаем и наполняем таблицу  набора данных
                DataSet ds = new DataSet();
                da.Fill(ds, "Visitor");
              
                ds.WriteXml(@"Visitor.xml");
                Console.WriteLine("OK");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            finally
            {
               conn.Close();
            }
           
        }

        /// <summary>
        /// Вставка в таблицу Visitor
        /// </summary>
        static void Task11()
        {
           
            // Создаем запрос на выборку
            string qry = @"select * from Visitor ";
            // Создаем запрос на вставку
            string ins = @"insert into Visitor (VID,FirstName,LastName,SecondName) values(@VID,@FirstName, @LastName,@SecondName)";
            // Создаем подключение
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                // Создаем команду запроса для текущего подключения	
                da.SelectCommand = new SqlCommand(qry, conn);
                // Создаем и наполняем набор данных
                DataSet ds = new DataSet();
                da.Fill(ds, "Visitor");
                // Получаем ссылку на таблицу 
                DataTable dt = ds.Tables["Visitor"];
                int tmp = (int)(dt.Rows[dt.Rows.Count - 1]["VID"])+1;

                // Добавляем  строку к таблице  набора данных 
                DataRow newRow = dt.NewRow();
                Console.WriteLine("FirstName: ");
                newRow["FirstName"] = Console.ReadLine();
                Console.WriteLine("LastName: ");
                newRow["LastName"] = Console.ReadLine();
                Console.WriteLine("SecondName: ");
                newRow["SecondName"] = Console.ReadLine();
                newRow["VID"] = tmp;
                dt.Rows.Add(newRow);
               
                
                // Создаем команду вставки для текущего подключения
                SqlCommand cmd = new SqlCommand(ins, conn);
                // Отображаем параметры
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 60, "FirstName");
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 60, "LastName");
                cmd.Parameters.Add("@SecondName", SqlDbType.NVarChar, 65, "SecondName");
                cmd.Parameters.Add("@VID", SqlDbType.Int, 4, "VID");

                // Адаптер выполняет команду вставки, обнаруженную в свойстве 'InsertCommand',
                da.InsertCommand = cmd;
                da.Update(ds, "Visitor");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e);
            }
            finally
            {
                // Закрываем подключение
                conn.Close();
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding= Encoding.Unicode;
            Console.InputEncoding= Encoding.Unicode;

            while (true)
            {
                Console.WriteLine("Выбор действия:");
                //1.	Работа с подсоединенными объектами:
                Console.WriteLine("1. проверка строки подключения");
                Console.WriteLine("2. выполнение скалярного запроса");
                Console.WriteLine("3. выполнение запроса, возвращающего набор строк");
                Console.WriteLine("4. выполнение параметризованных запросов на обновление");
                Console.WriteLine("5. выполнение хранимой процедуры без параметров");
                Console.WriteLine("6. выполнение хранимой процедуры с параметрами");
                //2 Работа с отсоединенными объектами:
                Console.WriteLine("7. наполнение таблицы набора данных результатами запроса");
                Console.WriteLine("8 работа с таблицами набора данных (фильтрация и сортировка)");
                Console.WriteLine("9. обновление данных в базе данных через адаптер данных");
                Console.WriteLine("10. сохранение данных набора данных в виде  XML-файла");
                Console.WriteLine("11. Вставка в таблицу");

                if(int.TryParse(Console.ReadLine(),out int result)==false)
                    continue;
                else
                {
                    switch (result)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("1. Информация о подключении к базе");
                           
                            Task1();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("2. Скалярный запрос выводящий количество поситителей");
                            Task2();
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("3. выполнение запроса, возвращающего набор строк");
                            Console.WriteLine("Список поситителей ID>1000");
                            Task3();
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("4. выполнение параметризованных запросов на обновление");
                            Console.WriteLine("Добавляем поситителя");
                            Task4();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("5. выполнение хранимой процедуры без параметров");
                            Console.WriteLine("Вызываем процедуру выводящюю список функций");
                            Task5();
                            break;
                        case 6:
                            Console.Clear();
                            Console.WriteLine("6. выполнение хранимой процедуры с параметрами");
                            Console.WriteLine("Встречи манеджера с № введенным с клавиатуры");
                            Task6();
                            break;
                        case 7:
                            Console.Clear();
                            Console.WriteLine("7. наполнение таблицы набора данных результатами запроса");
                            Console.WriteLine("Печатаеть поситителей с vid меньше 10");
                            Task7();
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("8 работа с таблицами набора данных (фильтрация и сортировка)");
                            Console.WriteLine("Фильтрация и сортировка по компании CompID < 5 And CompID > 1");
                            Task8();
                            break;
                        case 9:
                            Console.Clear();
                            Console.WriteLine("9. обновление данных в базе данных через адаптер данных");
                            Console.WriteLine("Обновляем номер компании у первого сотрудника из комании №3");
                            Task9();
                            break;
                        case 10:
                            Console.Clear();
                            Console.WriteLine("10. сохранение данных набора данных в виде  XML-файла");
                            Console.WriteLine("Выгрузка Visitor в XML");
                            Task10();
                            break;
                        case 11:
                            Console.Clear();
                            Console.WriteLine("11. Вставка в таблицу Visitor");
                            Task11();
                            break;
                    }
                }


            }
            
            Console.ReadKey();
        }
    }
}
