using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROC.Education.CSharpBotService.Storage
{
    /// <summary>
    /// Журнал регистрации студентов
    /// </summary>
    public class BotLog
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Регистрационное имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Номер телефона пользователя
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Метка времени записи
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
