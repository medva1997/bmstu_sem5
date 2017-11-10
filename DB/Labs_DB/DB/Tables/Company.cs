
namespace DB.Tables
{
    public class BaseTable
    {
        public static string Separator = ";";
    }

    public class Company
    {    /// <summary>
         /// Номер компании
         /// </summary>
        public int CompId;
        /// <summary>
        /// Главный менеджер
        /// </summary>
        public int CompAdminMid;
        /// <summary>
        /// Название компании
        /// </summary>
        public string CompName;
        /// <summary>
        /// Город
        /// </summary>
        public string CompCity;

        public override string ToString()
        {
            string sep = BaseTable.Separator;
            string line = "";
            line += CompId + sep;
            line += CompAdminMid + sep;
            line += CompName + sep;
            line += CompCity;
            return line;
        }
    }
}
