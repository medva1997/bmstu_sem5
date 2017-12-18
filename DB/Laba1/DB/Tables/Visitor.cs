
namespace DB.Tables
{
    public class Visitor
    {
        /// <summary>
        /// id	посетителя
        /// </summary>
        public int Vid;

        public User User;

        public override string ToString()
        {
            string sep = BaseTable.Separator;
            string line = "";
            line += Vid + sep;
            line +=User.ToString();
            return line;
        }
    }
}
