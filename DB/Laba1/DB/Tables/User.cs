
namespace DB.Tables
{
    public class User
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName;
        /// <summary>
        /// Отчество
        /// </summary>
        public string SecondName;
        /// <summary>
        /// Почта
        /// </summary>
        public string Email;
        /// <summary>
        /// Пароль	для	входа
        /// </summary>
        public string Password;
        /// <summary>
        /// Номер	телефона
        /// </summary>
        public string PhoneNumber;

        public override string ToString()
        {
            string sep = BaseTable.Separator;
            string line = "";
            line += LastName + sep;
            line += FirstName + sep;
            line += SecondName + sep;
            line += Email + sep;
            line += Password + sep;
            line += PhoneNumber;
            return line;
        }
    }
}
