using System;
using System.ComponentModel.DataAnnotations;


namespace CROC.EDUACATION.CSharpBotService.Storage
{
    public class Schedule
    {
        /// <summary>
        /// Ключ
        /// </summary>
        [Key()]
        public int Id { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// время начала
        /// </summary>
        public TimeSpan TimeStart { get; set; }
        /// <summary>
        /// время окончания
        /// </summary>
        public TimeSpan TimeEnd { get; set; }
        /// <summary>
        /// тема
        /// </summary>
        public string Topic { get; set; }

    }
}
