using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CROC.Education.CSharpBotService.Storage
{
    /// <summary>
    /// Студент (обучающийся)
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        [Key()]
        public Guid ID { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Required()]
        [Index("IX_UserID", IsUnique = true)]
        public int UserID { get; set; }

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
        /// Адрес электронной почты (обязателен для регистрации)
        /// </summary>
        [Required()]
        public string EMail { get; set; }

        /// <summary>
        /// Номер мобильного телефона 
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
