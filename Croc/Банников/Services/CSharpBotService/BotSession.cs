using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROC.Education.CSharpBotService
{
    /// <summary>
    /// Сеанс работы с ботом
    /// </summary>
    public class BotSession
    {
        /// <summary>
        /// Состояние бота
        /// </summary>
        public BotState State { get; set; }

        /// <summary>
        /// Метка времени изменения состояния
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Идентификатор беседы
        /// </summary>
        public long ChatID { get; set; }

        /// <summary>
        /// Номер телефона, отправленный в рамках сеанса
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="s">Начальное состояние (по умолчанию - None)</param>
        public BotSession(long chatId, BotState s = BotState.None)
        {
            ChatID = chatId;
            State = s;
            TimeStamp = DateTime.Now;
        }
    }
}
