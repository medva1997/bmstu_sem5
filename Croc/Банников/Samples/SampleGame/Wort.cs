using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SampleGame
{
    /// <summary>
    /// Словарная статья
    /// </summary>
    public class Wort
    {
        /// <summary>
        /// Слово
        /// </summary>
        [XmlAttribute(AttributeName = "Word")]
        public string Word;

        /// <summary>
        /// Автор
        /// </summary>
        [XmlAttribute(AttributeName = "Author")]
        public string Author;

        /// <summary>
        /// Слово одобрено модератором
        /// </summary>
        [XmlAttribute(AttributeName = "Approved")]
        public bool Approved;

        /// <summary>
        /// Количество слов, которые можно составить из данного слова
        /// 0 - для коротких слов        
        /// </summary>
        [XmlAttribute(AttributeName = "Count")]
        public int Count;

        /// <summary>
        /// Порядковый номер длинного слова
        /// 0 - для коротких слов
        /// </summary>
        [XmlAttribute(AttributeName = "Number")]
        public int Number;

        /// <summary>
        /// Дополнительный атрибут (его нет в файле, но мог бы быть)
        /// </summary>
        [XmlAttribute(AttributeName = "Extra")]
        public string Extra;
    }
}
