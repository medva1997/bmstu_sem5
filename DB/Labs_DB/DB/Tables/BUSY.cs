using System;

namespace DB.Tables
{
    public class Busy
    {
        /// <summary>
        /// номер записи
        /// </summary>
        public int Uid;
        /// <summary>
        /// Время начала слота в 15 мнит
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// 0 Не занято, 1-занято
        /// </summary>
        public byte Status;

        public override string ToString()
        {
            string sep = BaseTable.Separator;
            string line = "";
            line += Uid + sep;
            line += Time + sep;
            line += Status;
            return line;
        }
    }
}
