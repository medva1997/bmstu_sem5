using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using CROC.EDUACATION.CSharpBotService.Storage;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CROC.EDUACATION.CSharpBotService
{
    partial class CSharpBot
    {
        /// <summary>
        /// Клиент бота Telegram
        /// </summary>
        private readonly TelegramBotClient _client;

        /// <summary>
        /// База данных
        /// </summary>
        private readonly Db _db;

        /// <summary>
        /// Словарь состояний бота
        /// </summary>
        private readonly Dictionary<int, BotStates> _state;
        private readonly Dictionary<int, BotSession> _states2;

        /// <summary>
        /// таймер
        /// </summary>
        private System.Timers.Timer timer;

       
        /// <summary>
        /// Конструктор
        /// </summary>
        public CSharpBot()
        {
            string token = Properties.Settings.Default.Token;
            //XXXXXXXXXXXXXXXXXXXXXXXXXXXX
            _client = new TelegramBotClient(token);
            _client.OnMessage += MessageProc;
            _db = new Db();
            _state = new Dictionary<int, BotStates>();
            timer= new Timer();
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Событие таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime now= DateTime.Now;
            //Перебор всех состояний
            foreach (KeyValuePair<int, BotSession> pair in _states2)
            {
                //Проверка истечения времени
                if ((now - pair.Value.TimeStamp).TotalMinutes > 1 && pair.Value.state != BotStates.None)
                {
                    pair.Value.TimeStamp = now;
                    pair.Value.state = BotStates.None;
                    _client.SendTextMessageAsync(pair.Value.ChatId, "Похоже, вы про меня забыли");

                }
                
            }
        }

        /// <summary>
        /// Остановка бота
        /// </summary>
        public void Stop()
        {
            _client.StopReceiving();
            timer.Stop();
        }

        /// <summary>
        /// Запуск бота
        /// </summary>
        public void Start()
        {
            _client.StartReceiving();
            timer.Start();
        }


        /// <summary>
        /// Обработка получаемых сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProc(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {

                Console.WriteLine("{0} {1}: {2}", e.Message.From.FirstName, e.Message.From.LastName, e.Message.Text);

                switch (e.Message.Type)
                {
                    case MessageType.TextMessage:
                        TextProcessor(e.Message);
                        break;

                    case MessageType.ContactMessage:
                        ContactProcessor(e.Message);
                        break;

                    case MessageType.LocationMessage:
                        LocationProcessor(e.Message);
                        break;
                   
                    default:
                        _client.SendTextMessageAsync(e.Message.Chat.Id, "Я пока не понимаю это: " + e.Message.Type);
                        break;
                }
            }
            catch (Exception ex)
            {
                _client.SendTextMessageAsync(e.Message.Chat.Id, "У меня проблема: " + ex.Message);
            }
        }

        /// <summary>
        /// Обработка присланной геолокации
        /// </summary>
        /// <param name="msg"></param>
        private void LocationProcessor(Message msg)
        {
            string message;
            BotStates st = _state.ContainsKey(msg.From.Id) ? _state[msg.From.Id] : BotStates.None;
            switch (st)
            {
                // Вся геолокация легко обманывается на телефоне
                // В десктопной версии ее отправить нельзя
                // Инструкция по обходу геолокации
                //  1) вызываем /checkin
                //  2) нажимаем на скрепку и выбираем location
                //  3) Ищем на карте крок (удобно делать по раздвоению лефортовкого тоннеля)
                //  4) Отправляем
                //  Мы отметились :)
                case BotStates.Checkin:
                    //200 метров достачно даже для точности по базовым станциям
                    if (DistanceToCroc(msg) > 200)
                    {
                        message = "Ты явно не в КРОКе сейчас";
                        _client.SendTextMessageAsync(msg.Chat.Id, message);
                    }
                    else
                    {
                        //получаем текущий индекс занятия
                        int id = CheckLessonNow(msg, out var mes);
                        if (id!=0)
                        {
                            message = "Я передам что ты был на занятие";

                            _client.SendTextMessageAsync(msg.Chat.Id, message);
                            //отправка текущего занятия
                            _client.SendTextMessageAsync(msg.Chat.Id, mes);
                            //можно было бы это писать в Botlog, но там нужна совершенная новая структура,
                            //а если добавлять только location, то потом не удобно с этим работать
                            Checkin ch = new Checkin()
                            {
                                DateTime = DateTime.Now,
                                Latitude = msg.Location.Latitude,
                                Longitude = msg.Location.Longitude,
                                ScheduleId = id,
                                UserId = msg.From.Id
                            };
                            _db.Checkins.Add(ch);
                            _db.SaveChanges();
                            
                        }
                        else
                        {
                            //нет занятия
                            _client.SendTextMessageAsync(msg.Chat.Id, mes);
                            //Говорим когда будет следущее
                            NextLesson(msg);
                            //наверное стоит выводит за сколько открывается регистрация, 
                            //но для этого delta нужно делать глобальной
                        }
                    }
                    // возвращаем состояние
                    _state[msg.From.Id] = BotStates.None;
                    break;
                default:
                    message = "Я не знаю что делать c твоим местоположением.";
                    _client.SendTextMessageAsync(msg.Chat.Id, message);
                    break;
            }
        }

        /// <summary>
        /// Нужна ли пользователю регистрация на это занятие
        /// </summary>
        /// <param name="userId">ИД пользователя</param>
        /// <param name="lessonId">Номер занятия в расписании</param>
        /// <returns></returns>
        private bool NeedCheckIn(int userId, int lessonId)
        {
            //Можно склеить в один запрос, но так нагляднее и удобнее отлаживать
           var res =_db.Checkins.Where(c => c.UserId == userId);
           var res2 = res.Where(c => c.ScheduleId == lessonId);
            return !res2.Any();
        }

        /// <summary>
        /// Рассотояние до Крока в метрах
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private double DistanceToCroc(Message msg)
        {
            // вообще все сообщение тут не нужно, достаточно координат

            //координаты из яндекс карт
            //можно вынести в конфиг, чтобы быстрее изменять при изменение места проведения
            double crocLat = 55.753115 * (Math.PI / 180);
            double crocLong = 37.683124 * (Math.PI / 180);
            double latitude = msg.Location.Latitude * (Math.PI / 180); //широта
            double longitude = msg.Location.Longitude * (Math.PI / 180); //долгота

            //в метрах, формула из блога яндекса https://yandex.ru/blog/mapsapi/15001
            double dist = 6378137 * Math.Acos(Math.Cos(crocLat) * Math.Cos(latitude) * Math.Cos(longitude - crocLong) +
                                              Math.Sin(crocLat) * Math.Sin(latitude));
            return dist;
        }

        /// <summary>
        /// Обработка полученного контакта
        /// </summary>
        /// <param name="msg"></param>
        private void ContactProcessor(Message msg)
        {
            string message;
            BotStates st = _state.ContainsKey(msg.From.Id) ? _state[msg.From.Id] : BotStates.None;
            switch (st)
            {

                //Добавление номера при регистрации
                case BotStates.RegistrationPhone:

                    Student st2=_db.Students.FirstOrDefault(c => c.UserId == msg.From.Id);
                    //если что-то пошло не так и в базе пользователя нету к этому моменту
                    if (st2==null)
                    {
                        message ="Ошибка обновления данных";
                        _client.SendTextMessageAsync(msg.Chat.Id, message);
                    }
                    else
                    {
                        st2.PhoneNumber = msg.Contact.PhoneNumber;
                        _db.SaveChanges();

                        //Надо бы изменить форматирование текста
                        message = $"{msg.From.FirstName}, поздравляю, ты зарегистрирован. "+
                            " Теперь ты можешь отмечаться на занятии командой /checkin и получать информацию о занятии по команде /status";
                        _client.SendTextMessageAsync(msg.Chat.Id, message);
                    }
                    _state[msg.From.Id] = BotStates.None;
                    break;


                default:
                    message = $"{msg.From.FirstName}, мне приятно, что ты прислал мне номер телефона, но я не знаю что с ним делать.";
                    _client.SendTextMessageAsync(msg.Chat.Id, message);
                    break;
            }

            //Данная часть сдесь не нужна
            //Вообще смысл log не понятен, так как хранить что сообщение было, но без данных смысла не имеет
            //Имеет смысл хранить толко номер регистрации или userid
            //Storage.BotLog log = new Storage.BotLog()
                    //{
                    //    ID = Guid.NewGuid(),
                    //    Name = msg.Contact.FirstName,
                    //    FamilyName = msg.Contact.LastName,
                    //    UserUD = msg.Contact.UserId,
                    //    username = msg.From.Username,
                    //    dateTime = msg.Date,
                    //    PhoneNumber = msg.Contact.PhoneNumber
                    //};
                    //db.BotLogs.Add(log);
                    //db.SaveChanges();


            }

        /// <summary>
        /// Запрос номера телефона
        /// </summary>
        /// <param name="chatId">Id чата в телеграмме</param>
        private void RequestPhone(long chatId)
        {
            // Кнопка для отправки номера
            KeyboardButton b = new KeyboardButton("Отправить телефон")
            {
                RequestContact = true //отключение запроса номера
            };
            //KeyboardButton b2 = new KeyboardButton("Отмена")
            //{
            //    RequestContact = false //отключение запроса номера
            //};

            //панель кнопок
            KeyboardButton[] keys = { b };
            // Разметка ответа
            //Кнопка почему не одноразовая, она просто скрывается
            var markup = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(keys, true, true);

            // Ответ
            _client.SendTextMessageAsync(chatId, "Мне бы хотелось получить еще ваш номер телефона ", replyMarkup: markup);
        }

        /// <summary>
        /// Обработка текстовых сообщений
        /// </summary>
        /// <param name="msg">Сообщение</param>
        private void TextProcessor(Message  msg)
        {
            // Проверка на команду
            if (msg.Text.Substring(0,1)=="/")
            {
                CommandProcessor(msg, msg.Text.Substring(1));
            }
            else
            {
                // Определение текущего состояния бота
                BotStates st = _state.ContainsKey(msg.From.Id) ? _state[msg.From.Id] : BotStates.None;
                // Селектор состояний
                switch (st)
                {
                   
                    case BotStates.RegistrationEmail:
                        RegistrationEmail(msg);
                        break;

                    default:
                        // стоит добавить вывод всех команд
                        var message = $"Привет, {msg.From.FirstName}";
                        _client.SendTextMessageAsync(msg.Chat.Id, message);
                        break;

                }
                
            }
            
        }

        /// <summary>
        /// Регистрация почты
        /// </summary>
        /// <param name="msg">Сообщение</param>
        private void RegistrationEmail(Message msg)
        {
            string message;
            bool emailfound = false;
            foreach (var entity in msg.Entities)
            {
                if (entity.Type == MessageEntityType.Email)
                {
                    // E-Mail нашелся
                    Student student = new Student()
                    {
                        Id = Guid.NewGuid(),
                        UserId = msg.From.Id,
                        Name = msg.From.FirstName,
                        FamilyName = msg.From.LastName,
                        UserName = msg.From.Username,
                        EMail = msg.Text.Substring(entity.Offset, entity.Length),
                        ChatId = msg.Chat.Id
                        //номер телефона оставляем пустыми
                    };
                    //В базу
                    _db.Students.Add(student);
                    _db.SaveChanges();

                    message = $"Спасибо, {msg.From.FirstName}. У тебя классная почта!";
                    _client.SendTextMessageAsync(msg.Chat.Id, message);
                    emailfound = true;

                    // Переход к запросу телефона
                    _state[msg.From.Id] = BotStates.RegistrationPhone;
                    RequestPhone(msg.Chat.Id);
                    break;
                }
            }
            if (!emailfound)
            {
                message = $"{msg.From.FirstName}, для завершения регистрации отправьте, пожалуйста, E-mail";
                _client.SendTextMessageAsync(msg.Chat.Id, message);
            }
        }

        

        /// <summary>
        /// Нужно ли зарегестрироваться
        /// </summary>
        /// <param name="id">UserId из телеграмма</param>
        /// <returns></returns>
        private bool NeedRegistration(long id)
        {
            Student check = _db.Students.FirstOrDefault(c => c.UserId == id);
            if (check==null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Обработка текстовой команды
        /// </summary>
        /// <param name="msg">Сообщение</param>
        /// <param name="cmd">Команда</param>
        private void CommandProcessor(Message msg, string cmd)
        {
            // Селектор команд без учета регистра букв
            switch (cmd.ToLower())
            {
                case "checkin":
                    CheckinCommand(msg);
                    break;

                case "start":
                    StartCommand(msg);
                    break;

                case "registration":
                    RegistrationCommand(msg);
                    break;
                case "status":
                    StatusCommand(msg);
                    break;

                default:
                    DefaultCommand(msg, cmd);
                    break;
            }

        }
        /// <summary>
        /// Добавление занятия в расписание
        /// </summary>
        /// <param name="sc"> элемент расписания</param>
        public void AddLesson(Schedule sc)
        {
            _db.Schedules.Add(sc);
            _db.SaveChanges();

        }
        
    }
}
