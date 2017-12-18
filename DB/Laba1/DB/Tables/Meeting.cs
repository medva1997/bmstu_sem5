using System;


namespace DB.Tables
{
    public class Meeting
    {
        /// <summary>
        /// ИД	встречи
        /// </summary>
        public int MeetId;
        /// <summary>
        /// Номер	менеджера
        /// </summary>
        public int Mid;
        /// <summary>
        /// Номер	посетителя
        /// </summary>
        public int Vid;
        /// <summary>
        /// Время	начала
        /// </summary>
        public DateTime TimeStart;
        /// <summary>
        /// Время	концa
        /// </summary>
        public DateTime TimeEnd;
        /// <summary>
        /// Подтверждение	от	менеджера
        /// </summary>
        public int MConfirmation;
        /// <summary>
        /// Подтверждение	от	пользователя
        /// </summary>
        public int VConfirmation;
        /// <summary>
        /// Место
        /// </summary>
        public string Location;

        public override string ToString()
        {
            string sep = BaseTable.Separator;
            string line = "";
            line += MeetId + sep;
            line += Mid + sep;
            line += Vid + sep;
            line += TimeStart + sep;
            line += TimeEnd + sep;
            line += MConfirmation + sep;
            line += VConfirmation + sep;
            line += Location;
            return line;
        }

    }
}
