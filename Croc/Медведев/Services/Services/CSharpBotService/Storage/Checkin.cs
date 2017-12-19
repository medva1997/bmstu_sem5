using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROC.EDUACATION.CSharpBotService.Storage
{
    public class Checkin
    {
        /// <summary>
        /// уникальный номер
        /// </summary>
        [Key()]
        public int Id { get; set; }
        /// <summary>
        /// Номер пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// широта
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// долгота
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Номер занятия в расписании
        /// </summary>
        public int ScheduleId { get; set; }

    }
}
