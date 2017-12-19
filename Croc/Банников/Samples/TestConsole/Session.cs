using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Session : IEnumerable
    {
        private Dictionary<string, string> dict;

        public Session()
        {
            dict = new Dictionary<string, string>();
        }

        /// <summary>
        /// Индексируемое свойство
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string this[string s]
        {
            get
            {
                if (dict.ContainsKey(s))
                {
                    return dict[s];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (dict.ContainsKey(s))
                {
                    dict[s] = value;
                }
                else
                {
                    dict.Add(s, value);
                }
            }
        }            

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }
    }
}

