using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapService
{
    public class Result
    {
        public long? Value;

        public bool Valid;

        public string Message;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Result()
        {
            Value = null;
            Valid = false;
            Message = null;
        }

        /// <summary>
        /// Конструктор по значению
        /// </summary>
        /// <param name="v"></param>
        public Result(long v)
        {
            Value = v;
            Valid = true;
            Message = null;
        }

        /// <summary>
        /// Конструктор по исключению
        /// </summary>
        /// <param name="ex">Исключение</param>
        public Result(Exception ex)
        {
            Value = null;
            Valid = false;
            // Текст исключения вместе с его типом
            Message = string.Format("{0}: {1}", ex.GetType().FullName, ex.Message);
        }

        /// <summary>
        /// Конструктор по строке
        /// </summary>
        /// <param name="s">Сообщение</param>
        public Result(string s)
        {
            Value = null;
            Valid = false;
            Message = s;
        }
    }
}