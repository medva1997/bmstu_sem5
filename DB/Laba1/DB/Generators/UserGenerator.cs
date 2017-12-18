using System;
using System.Collections.Generic;
using System.Text;
using DB.Tables;

namespace DB.Generators
{
    class UserGenerator:BaseGenerator<User>
    {
        /// <summary>
        /// Массив со всеми возможными фамилиями
        /// </summary>
        private readonly string[] _snames;
        /// <summary>
        /// Массив со всеми возможными именами
        /// </summary>
        private readonly string[] _fnames;
        /// <summary>
        /// Массив со всеми возможными отчествами
        /// </summary>
        private readonly string[] _mnames;

        /// <summary>
        /// Массив со всеми возможными фамилиями
        /// </summary>
        private readonly string[] _snames2;
        /// <summary>
        /// Массив со всеми возможными именами
        /// </summary>
        private readonly string[] _fnames2;
        /// <summary>
        /// Массив со всеми возможными отчествами
        /// </summary>
        private readonly string[] _mnames2;
        /// <summary>
        /// количество строк в каждом пункте
        /// </summary>
        private readonly int _sN, _fN, _mN, _sN2, _fN2, _mN2;

        /// <summary>
        /// рандом
        /// </summary>
        private readonly Random _rnd;

        public UserGenerator(string path, int nUsers)
        {
            _rnd = new Random();
            _snames = System.IO.File.ReadAllLines(path + "snames1.txt");
            _fnames = System.IO.File.ReadAllLines(path + "fnames1.txt");
            _mnames = System.IO.File.ReadAllLines(path + "mnames1.txt");
            _sN = _snames.Length;
            _fN = _fnames.Length;
            _mN = _mnames.Length;

            _snames2 = System.IO.File.ReadAllLines(path + "snames2.txt");
            _fnames2 = System.IO.File.ReadAllLines(path + "fnames2.txt");
            _mnames2 = System.IO.File.ReadAllLines(path + "mnames2.txt");
            _sN2 = _snames2.Length;
            _fN2 = _fnames2.Length;
            _mN2 = _mnames2.Length;

            init_dict();
            Generator(nUsers);
        }


        private void Generator(int nUsers)
        {
            for (int i = 0; i < nUsers; i++)
            {
                Generate();
            }
            
        }

        /// <summary>
        /// Генератор ФИО
        /// </summary>
        private void Generate()
        {
            string sname, fname, lname;

            int gen = _rnd.Next(2);
            if (gen == 0)
            {
                lname = _snames[_rnd.Next(_sN)];
                fname = _fnames[_rnd.Next(_fN)];
                sname = _mnames[_rnd.Next(_mN)];
                //gender = "мужской";
            }
            else
            {
                lname = _snames2[_rnd.Next(_sN2)];
                fname = _fnames2[_rnd.Next(_fN2)];
                sname = _mnames2[_rnd.Next(_mN2)];
                //gender = "женский";
            }

            string mail = Tranclite(lname) + "@medalexey.ru";
            string pass = CreateMd5(lname+fname+sname);
            long phone = 79260000000+GenerateNdigitsN(7);

            Lst.Add(new User(){FirstName = fname,LastName = lname,SecondName = sname,Email = mail,Password = pass,PhoneNumber = phone.ToString()});


        }

        public static string CreateMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.Unicode.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        Dictionary<char, string> words = new Dictionary<char, string>();

        private string Tranclite (string source)
        {

            source = source.ToLower();
            string destiny = "";
            foreach (char t in source)
            {
                if (words.ContainsKey(t))
                    destiny = destiny + words[t];
                else
                    destiny = destiny + t;
            }

            StringBuilder sb = new StringBuilder(destiny);
            for (int j = 0; j < sb.Length; j++)
            {
                if (char.IsLower(sb[j]))
                    sb[j] = char.ToLower(sb[j]);
                else if (char.IsUpper(sb[j]))
                    sb[j] = char.ToUpper(sb[j]);
            }
            string corrected = sb.ToString();


            return corrected;
        }


        private void init_dict()
        {
            words.Add('а', "a");
            words.Add('б', "b");
            words.Add('в', "v");
            words.Add('г', "g");
            words.Add('д', "d");
            words.Add('е', "e");
            words.Add('ё', "yo");
            words.Add('ж', "zh");
            words.Add('з', "z");
            words.Add('и', "i");
            words.Add('й', "j");
            words.Add('к', "k");
            words.Add('л', "l");
            words.Add('м', "m");
            words.Add('н', "n");
            words.Add('о', "o");
            words.Add('п', "p");
            words.Add('р', "r");
            words.Add('с', "s");
            words.Add('т', "t");
            words.Add('у', "u");
            words.Add('ф', "f");
            words.Add('х', "h");
            words.Add('ц', "c");
            words.Add('ч', "ch");
            words.Add('ш', "sh");
            words.Add('щ', "sch");
            words.Add('ъ', "j");
            words.Add('ы', "i");
            words.Add('ь', "j");
            words.Add('э', "e");
            words.Add('ю', "yu");
            words.Add('я', "ya");
            
        }
    }
}
