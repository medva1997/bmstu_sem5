using System;


namespace DB
{
    class WorkHours
    {
        /// <summary>
        /// Первый день работы
        /// </summary>
        public DateTime EventStart;
        /// <summary>
        /// Последний день работы
        /// </summary>
        public DateTime EventEnd;
        /// <summary>
        /// Начало рабочего дня
        /// </summary>
        public TimeSpan DayStart;
        /// <summary>
        /// Конец рабочего дня
        /// </summary>
        public TimeSpan DayEnd;
        /// <summary>
        /// Шаг для генерарации
        /// </summary>
        public TimeSpan Step;

    }
}
