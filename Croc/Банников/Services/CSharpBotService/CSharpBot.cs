using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CROC.Education.CSharpBotService
{
    /// <summary>
    /// Бизнес-логика чат-бота
    /// </summary>
    class CSharpBot
    {
        /// <summary>
        /// Клиент бота Telegram
        /// </summary>
        private TelegramBotClient client;

        /// <summary>
        /// База данных
        /// </summary>
        private Storage.DB db;

        /// <summary>
        /// Словарь состояний бота
        /// </summary>
        private Dictionary<int, BotSession> state;

        /// <summary>
        /// Таймер для состояний
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Конструктор
        /// </summary>
        public CSharpBot()
        {
            string token = Properties.Settings.Default.Token;
            client = new TelegramBotClient(token);
            client.OnMessage += MessageProcessor;
            db = Storage.DB.Open();
            state = new Dictionary<int, BotSession>();
            timer = new System.Timers.Timer();
            // 1 секунда
            timer.Interval = 1000;
            // Многократный запуск
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Событие срабатывания таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Текущее время
            DateTime now = DateTime.Now;
            // Перебор всех состояний
            foreach (KeyValuePair<int, BotSession> pair in state)
            {
                // Проверка истечения таймаута состояния 
                // Таймаут задан в конфигурации в секундах
                if (((now - pair.Value.TimeStamp).TotalSeconds > Properties.Settings.Default.StateTimeout) && (pair.Value.State != BotState.None))
                {
                    client.SendTextMessageAsync(pair.Value.ChatID, "Похоже, вы про меня забыли, попробуйте позже");
                    pair.Value.State = BotState.None;
                    pair.Value.TimeStamp = now;
                }
            }
        }

        /// <summary>
        /// Запуск бота
        /// </summary>
        public void Start()
        {
            client.StartReceiving();
            timer.Start();
        }

        /// <summary>
        /// Останов бота
        /// </summary>
        public void Stop()
        {
            client.StopReceiving();
            timer.Stop();
        }

        /// <summary>
        /// Текущее состояние пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        private BotState GetState(int userId)
        {
            return state.ContainsKey(userId) ? state[userId].State : BotState.None;
        }

        /// <summary>
        /// Установить состояние пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="newState">Новое состояние</param>
        private void SetState(int userId, BotState newState = BotState.None)
        {
            // Проверка на существование состояния
            if (!state.ContainsKey(userId))
            {
                state[userId] = new BotSession(userId, newState);
            }
            else
            {
                // Установка нового состояния и обновление метки времени
                state[userId].State = newState;
                state[userId].TimeStamp = DateTime.Now;
            }
        }

        /// <summary>
        /// Обработка получаемых сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProcessor(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {
                // Протоколирование (если разрешено в конфигурации)
                if (Properties.Settings.Default.Verbose)
                {
                    Console.WriteLine("{0} {1}: {2}", e.Message.From.FirstName, e.Message.From.LastName, e.Message.Text);
                }
                // Проверка типа сообщения
                // Если в классе присутствует публичный метод с именем <тип сообщения>Processor
                // он будет вызван как обработчик сообщения соответствующего типа, иначе
                // будет выведено информационное сообщение                   
                string processor = e.Message.Type.ToString() + "Processor";
                System.Reflection.MethodInfo info = GetType().GetMethod(processor);
                if (info != null)
                {
                    // Вызов метода по имени с заданным параметром
                    info.Invoke(this, new object[] { e.Message });
                }
                else
                {
                    client.SendTextMessageAsync(e.Message.Chat.Id, "Я пока не понимаю это: " + e.Message.Type);
                }
            }
            catch (Exception ex)
            {
                client.SendTextMessageAsync(e.Message.Chat.Id, "К сожалению, у меня проблема: " + ex.Message);
                // Вывод всех вложенных исключений
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    client.SendTextMessageAsync(e.Message.Chat.Id, "Дополнительная информация: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Обработка текстовых сообщений
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void TextMessageProcessor(Message msg)
        {
            string message;

            // Проверка на команду
            if (msg.Text.Substring(0, 1) == "/")
            {
                CommandProcessor(msg, msg.Text.Substring(1));
            }
            else
            {
                // Селектор состояний
                switch (GetState(msg.From.Id))
                {
                    case BotState.None:
                        message = string.Format("Привет, {0}!", msg.From.FirstName);
                        client.SendTextMessageAsync(msg.Chat.Id, message);
                        break;

                    case BotState.Registration:
                        if (string.IsNullOrEmpty(state[msg.From.Id].PhoneNumber))
                        {
                            client.SendTextMessageAsync(msg.Chat.Id, "Отправьте, пожалуйста, Ваш номер телефона");
                            break;
                        }
                        bool emailFound = false;
                        foreach (var entity in msg.Entities)
                        {
                            if (entity.Type == Telegram.Bot.Types.Enums.MessageEntityType.Email)
                            {
                                // Регистрация пользователя
                                Storage.Student student = new Storage.Student()
                                {
                                    ID = Guid.NewGuid(),
                                    UserID = msg.From.Id,
                                    Name = msg.From.FirstName,
                                    FamilyName = msg.From.LastName,
                                    UserName = msg.From.Username,
                                    EMail = msg.Text.Substring(entity.Offset, entity.Length),
                                    PhoneNumber = state[msg.From.Id].PhoneNumber
                                };
                                db.Students.Add(student);
                                db.SaveChanges();
                                // E-Mail нашелся
                                message = string.Format("Спасибо, {0}, регистрация выполнена!", msg.From.FirstName);
                                client.SendTextMessageAsync(msg.Chat.Id, message);
                                emailFound = true;
                                // Возвращение к базовому состоянию
                                SetState(msg.From.Id, BotState.None);
                                break;
                            }
                        }
                        if (!emailFound)
                        {
                            message = string.Format("{0}, для завершения регистрации отправьте, пожалуйста, E-mail", msg.From.FirstName);
                            client.SendTextMessageAsync(msg.Chat.Id, message);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Обработка текстовой команды
        /// </summary>
        /// <param name="msg">Сообщение</param>
        /// <param name="cmd">Команда</param>
        public void CommandProcessor(Message msg, string cmd)
        {
            // Имя метода для обработки команды
            string name = string.Format("{0}Command", cmd.ToLower());
            System.Reflection.MethodInfo info = GetType().GetMethod(name);
            if (info != null)
            {
                // Вызов метода по имени с заданным параметром
                info.Invoke(this, new object[] { msg });
            }
            else
            {
                client.SendTextMessageAsync(msg.Chat.Id, "Неизвестная команда: " + cmd);
            }
        }

        #region "Обработчики команд"

        /// <summary>
        /// Команда начала работы с ботом
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void startCommand(Message msg)
        {
            client.SendTextMessageAsync(msg.Chat.Id, string.Format("Привет, {0}, я учебный бот по языку C#", msg.From.FirstName));
        }

        /// <summary>
        /// Отметить присутствие студента на занятии
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void checkinCommand(Message msg)
        {
            // Кнопка для регистрации
            KeyboardButton b = new KeyboardButton("Отметиться")
            {
                RequestLocation = true
            };
            // Панель кнопок
            KeyboardButton[] keys = new KeyboardButton[1] { b };
            // Разметка ответа
            var markup = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(keys, true, true);
            // Ответ
            client.SendTextMessageAsync(msg.Chat.Id, "Предоставьте регистрационные данные", replyMarkup: markup);
        }

        /// <summary>
        /// Зарегистрировать студента в списке
        /// </summary>
        /// <param name="msg">Сообщение</param>
        public void registerCommand(Message msg)
        {
            Storage.Student student = db.Students.Where(a => a.UserID == msg.From.Id).FirstOrDefault();
            if (student != null)
            {
                client.SendTextMessageAsync(msg.Chat.Id, string.Format("{0}, вы уже зарегистрированы, ваш E-mail: {1}. Для редактирования профиля используется команда /profile", msg.From.FirstName, student.EMail));
            }
            else
            {
                // Кнопка для регистрации с запросом номера телефона
                KeyboardButton b = new KeyboardButton("Отправить телефон")
                {
                    RequestContact = true
                };
                // Панель кнопок
                KeyboardButton[] keys = new KeyboardButton[1] { b };
                // Разметка ответа
                var markup = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(keys, true, true);
                client.SendTextMessageAsync(msg.Chat.Id, "Для регистрации отправьте мне телефон и после него E-mail", replyMarkup: markup);
                // Переход в состояние регистрации пользователя
                SetState(msg.From.Id, BotState.Registration);
                state[msg.From.Id].PhoneNumber = null;
            }
        }

        #endregion

        #region "Обработчики различных типов сообщений"

        /// <summary>
        /// Обработка полученного контакта
        /// </summary>
        /// <param name="msg"></param>
        public void ContactMessageProcessor(Message msg)
        {
            switch (GetState(msg.From.Id))
            {
                case BotState.Registration:
                    if (!string.IsNullOrEmpty(state[msg.From.Id].PhoneNumber))
                    {
                        client.SendTextMessageAsync(msg.Chat.Id, "Вы уже отправляли номер телефона, теперь отправьте Ваш E-mail");
                    }
                    else
                    {
                        state[msg.From.Id].PhoneNumber = msg.Contact.PhoneNumber;
                        client.SendTextMessageAsync(msg.Chat.Id, "Спасибо, теперь отправьте Ваш E-mail");
                    }
                    break;

                default:
                    client.SendTextMessageAsync(msg.Chat.Id, "Извините, не понимаю, что надо сделать с этим контактом");
                    break;
            }
        }

        /// <summary>
        /// Обработка метки геолокации
        /// </summary>
        /// <param name="msg">Сообщение с меткой геолокации</param>
        public void LocationMessageProcessor(Message msg)
        {
        }

        /// <summary>
        /// Обработка документа
        /// </summary>
        /// <param name="msg">Сообщение с документом</param>
        public void DocumentMessageProcessor(Message msg)
        {
            string s = string.Format("Я получил файл {0}", msg.Document.FileName);
            client.SendTextMessageAsync(msg.Chat.Id, s);
        }
        #endregion

    }
}
