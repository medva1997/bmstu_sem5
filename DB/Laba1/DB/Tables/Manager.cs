

namespace DB.Tables
{
    class Manager
    {
        /// <summary>
        /// id	посетителя
        /// </summary>
        public int Mid;
        /// <summary>
        /// ссылка на компанию
        /// </summary>
        public int CompId;

        public User User;
       

        public override string ToString()
        {
            string sep = BaseTable.Separator;
            string line = "";
            line += Mid + sep;
            line += CompId + sep;
            line += User.ToString();
            return line;
        }
    }
}
