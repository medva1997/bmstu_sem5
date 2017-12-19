using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CROC.EDUACATION.CSharpBotService.Storage
{
    public class Student
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        [Key()]
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Required()]
        [Index("IX_UserID", IsUnique = true)]
        public int UserId { get; set; }

        /// <summary>
        /// Номер чата если мы решим делать рассылку
        /// </summary>
        public long ChatId { get; set; }

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
        /// Адрес электронной почты
        /// </summary>
        [Required()]
        public string EMail { get; set; }

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
