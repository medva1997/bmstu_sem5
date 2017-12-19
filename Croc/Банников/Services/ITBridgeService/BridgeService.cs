using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ITBridgeService
{
    /// <summary>
    /// Сервис
    /// </summary>
    partial class BridgeService : ServiceBase
    {
        /// <summary>
        /// Номер порта
        /// </summary>
        private const int port = 4500;

        /// <summary>
        /// Точка вызова
        /// </summary>
        IPEndPoint ipPoint;

        /// <summary>
        /// Сокет TCP/IP
        /// </summary>
        Socket listenSocket;

        /// <summary>
        /// Основной поток сервиса
        /// </summary>
        Thread thread;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        public BridgeService()
        {
            InitializeComponent();

            ipPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(ipPoint);
            listenSocket.Listen(10);
            thread = new Thread(Worker);
        }

        /// <summary>
        /// Рабочий поток
        /// </summary>
        private void Worker()
        {
            while (true)
            {
                try
                {
                    Socket handler = listenSocket.Accept();

                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    // Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                    string message = builder.ToString();
                    ///разбиваем сообщение на части

                    string message_type = message.Split(new char[] { '#' }).ElementAt(0);

                    switch (message_type)
                    {
                        case "manager":
                            new_manager(message);
                            break;
                        case "request":
                            send_request_to_manager(message);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                }
                // Пауза 100 миллисекунд
                Thread.Sleep(100);
            }
        }

                
        private void send_request_to_manager(string message)
        {
            Console.WriteLine(DateTime.Now.ToShortTimeString() + ":" + message);
            int port = 4501;
            /// выбираем IP-адрес из строки
            string address = "192.168.102.23";
            try
            {
                ///формируем IP-адрес из строки
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                /// инициализируем сетевое соединение и передаем сообщение
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(ipPoint);
                String host = System.Net.Dns.GetHostName();
                System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];

                /// формируем сообщение и отправляем в открытый сокет
                //string message = "request#" + cabnumber + "#" + RequestAddInfo.Text;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void new_manager(string message)
        {
            /// пытаемся записать в файл с конфигурацией
            try
            {
                string str = string.Empty;
                string result_str = string.Empty;

                string tempcab = message.Split(new char[] { '#' }).ElementAt(2);
                Console.WriteLine(DateTime.Now.ToShortTimeString() + ":" + tempcab);

                /// проверяем, есть ли файл с конфигурацией
                if (File.Exists("config.dat"))
                {
                    using (StreamReader sr = new StreamReader("config.dat"))
                    {
                        //читаем построчно и находим строку с номером кабинета
                        string sLine = "";
                        while ((sLine = sr.ReadLine()) != null)
                        {
                            if (sLine.Contains("#" + tempcab + "#"))
                                str = sLine;
                        }
                        sr.Close();
                    }

                    // если есть, открываем и читаем из него все данные, заменяем на новые данные + дата
                    using (System.IO.StreamReader sre = System.IO.File.OpenText("config.dat"))
                    {
                        string tempstr = sre.ReadToEnd();
                        if (str != "")
                        {
                            result_str = tempstr.Replace(str, message + "#" + DateTime.Now.ToShortDateString());
                        }
                        else
                        {
                            result_str = tempstr + "\r\n" + message + "#" + DateTime.Now.ToShortDateString();
                        }
                        sre.Close();
                    }
                    ///записываем результаты в файл с обновленными данными
                    using (System.IO.StreamWriter wr = new System.IO.StreamWriter("config.dat"))
                    {
                        wr.Write(result_str);
                        wr.Close();
                    }
                }
                else
                {
                    /// создаем файл конфигурации и записываем в него значения
                    System.IO.StreamWriter write = File.CreateText("config.dat");
                    write.Write(message + "#" + DateTime.Now.ToShortDateString());
                    write.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Не могу записать в файл с конфигурацией - Ошибка записи конфигурации");
            }
        }

        /// <summary>
        /// Запуск сервиса
        /// </summary>
        public void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// Останов сервиса
        /// </summary>
        public void Stop()
        {
            thread.Abort();
        }

        /// <summary>
        /// Событие запуска сервиса
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Start();
        }

        /// <summary>
        /// Событие останова сервиса
        /// </summary>
        protected override void OnStop()
        {
            Stop();
        }
    }
}
