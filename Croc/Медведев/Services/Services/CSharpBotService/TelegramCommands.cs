using System;
using System.Linq;
using CROC.EDUACATION.CSharpBotService.Storage;
using Telegram.Bot.Types;

namespace CROC.EDUACATION.CSharpBotService
{
    partial class CSharpBot
    {
        /// <summary>
        /// Команда Checkin
        /// </summary>
        /// <param name="msg">Сообщение</param>
        private void CheckinCommand(Message msg)
        {
            // Проверка на наличие регистрации
            if (NeedRegistration(msg.From.Id))
            {
                _client.SendTextMessageAsync(msg.Chat.Id,
                    " Для того чтобы отметиться тебе сначала нужно рассказать о себе. Для этого нажми /registration");
                return;
            }

            //Узнаем какой сейчас урок идет и проверяем наличие уже чекина на этот урок
            var id = CheckLessonNow(msg, out var _);
            if (!NeedCheckIn(msg.From.Id, id))
            {
                _client.SendTextMessageAsync(msg.Chat.Id, "Ты уже отметился");
                return;
            }

            //Ответ бота

            // Кнопка для регистрации
            KeyboardButton b = new KeyboardButton("Отметиться")
            {
                RequestLocation = true
                //b.RequestContact = true; //отключение запроса номера
            };


            //панель кнопок
            KeyboardButton[] keys = {b};
            // Разметка ответа
            var markup = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(keys, true, true);
            // Ответ
            _client.SendTextMessageAsync(msg.Chat.Id, "Предоставьте свое местоположение", replyMarkup: markup);
            _state[msg.From.Id] = BotStates.Checkin;
        }

        /// <summary>
        /// Команда START
        /// </summary>
        /// <param name="msg">Сообщение</param>
        private void StartCommand(Message msg)
        {
            _client.SendTextMessageAsync(msg.Chat.Id, $"Привет, {msg.From.FirstName}, я учебный бот по языку C#. " +
                                                      "Для того чтобы рассказать о себе нажми /registration");
        }

        /// <summary>
        /// Заглушка для неопознанных команд 
        /// </summary>
        /// <param name="msg">Сообщение</param>
        /// <param name="cmd">Команда</param>
        private void DefaultCommand(Message msg, string cmd)
        {
            _client.SendTextMessageAsync(msg.Chat.Id, "Bad command " + cmd);
        }

        /// <summary>
        /// Команда Регистрации
        /// </summary>
        /// <param name="msg">Сообщение</param>
        private void RegistrationCommand(Message msg)
        {
            if (NeedRegistration(msg.From.Id))
            {
                _client.SendTextMessageAsync(msg.Chat.Id, "Для регистрации отправьте мне E-mail.");
                // Переход в состояние регистрации пользователя
                _state[msg.From.Id] = BotStates.RegistrationEmail;
            }
            else
            {
                _client.SendTextMessageAsync(msg.Chat.Id, "Мы уже знакомились и повторная регистрация не нужна");
            }
        }
        /// <summary>
        /// Команда status
        /// </summary>
        /// <param name="msg"></param>
        private void StatusCommand(Message msg)
        {
            if (CheckLessonNow(msg, out var mes) != 0)
            {
                _client.SendTextMessageAsync(msg.Chat.Id, mes);
            }
            else
            {
                _client.SendTextMessageAsync(msg.Chat.Id, mes);
                NextLesson(msg);
            }
        }

        /// <summary>
        /// Есть ли сейчас занятие с люфтом в 15 минут до и после занятия
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="message"> сообщение для печати</param>
        /// <returns></returns>
        public int CheckLessonNow(Message msg, out string message)
        {
            DateTime today = DateTime.Today.Date;
            TimeSpan delta = new TimeSpan(0, 15, 0);

            // можно было бы не заводить эти две переменные, но почему-то в запросе он не хочет их вычислять
            // можно задать разную delta до и после занятия, например 30 минут и 3 часа соответвенно
            TimeSpan nowTimeSpan1 = DateTime.Now.TimeOfDay + delta;
            TimeSpan nowTimeSpan2 = DateTime.Now.TimeOfDay - delta;


            Schedule res = _db.Schedules.FirstOrDefault(c =>
                c.Date == today && c.TimeStart <= nowTimeSpan1 && c.TimeEnd > nowTimeSpan2);
            if (res == null)
            {
                message = "Сейчас нет занятия";
                return 0;
            }
            //можно засунуть в else, но не обязательно
            message = "Сейчас идет занятие по теме " + res.Topic;

            return res.Id;
        }

        /// <summary>
        /// Узнаем и отправляем сообщение со следующим занятием
        /// </summary>
        /// <param name="msg"></param>
        public void NextLesson(Message msg)
        {
            DateTime today = DateTime.Today.Date;
            Schedule lesson = _db.Schedules.OrderBy(c => c.Date).FirstOrDefault(c => c.Date >= today);
            if (lesson != null)
            {
                if (lesson.Date + lesson.TimeStart > DateTime.Now)
                {
                    string line = $"{lesson.Date:d MMMM}";
                    _client.SendTextMessageAsync(msg.Chat.Id,
                        $"Следующее занятие будет  {line}  в {lesson.TimeStart.Hours} часов {lesson.TimeStart.Minutes} минут, тема занятия {lesson.Topic}");
                }
            }
        }
    }
}