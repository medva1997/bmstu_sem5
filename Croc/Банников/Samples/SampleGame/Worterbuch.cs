using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SampleGame
{
    /// <summary>
    /// Словарь
    /// </summary>
    [XmlRoot(ElementName = "Dict", Namespace = "http://www.orioner.ru/croc")]
    public class Worterbuch
    {
        /// <summary>
        /// Массив словарный слов
        /// </summary>
        [XmlElement(ElementName = "Item")]
        public Wort[] Words;

        /// <summary>
        /// Загрузка XML-файла десериализацией
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public Worterbuch Load(string path)
        {
            // Сериалиазтор
            XmlSerializer ser = new XmlSerializer(typeof(Worterbuch));
            // Читатель
            XmlReader rdr = XmlReader.Create(path);
            // Выполнить десериализацию и вернуть результат в виде объекта
            return (Worterbuch)ser.Deserialize(rdr);
        }

        /// <summary>
        /// Вернуть длинное слово по номеру
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string GetLongWord(int n)
        {
            string s = Words.Where(a => a.Number == n).First().Word;
            return s;

            /* эквивалентный пример кода
            string w;
            foreach (Wort v in Words)
            {
                if (v.Number == n)
                {
                    w = v.Word;
                    break;
                }
            }
            */
        }

        /// <summary>
        /// Подсчет количества длинных слов в словаре
        /// </summary>
        /// <returns></returns>
        public int LongWordCount()
        {
            return Words.Where(a => a.Number > 0).Count();
        }
    }
}
