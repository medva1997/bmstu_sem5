using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfConsole
{
    public class Result
    {
        public string ID;

        public long? Value;

        public bool Valid;

        public string Message;

        public Error Ex;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Result()
        {
            Value = null;
            Valid = false;
            Message = null;
            ID = null;
            Ex = null;
        }

        /// <summary>
        /// Конструктор по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public Result(Guid id)
        {
            ID = id.ToString("N");
        }

        /// <summary>
        /// Конструктор по значению
        /// </summary>
        /// <param name="v"></param>
        public Result(long v, Guid id) : this(id)
        {
            Value = v;
            Valid = true;
            Message = null;
            Ex = null;
        }

        /// <summary>
        /// Конструктор по исключению
        /// </summary>
        /// <param name="ex">Исключение</param>
        public Result(Exception ex, Guid id) : this(id)
        {
            Value = null;
            Valid = false;
            // Текст исключения вместе с его типом
            Message = string.Format("{0}: {1}", ex.GetType().FullName, ex.Message);
            Ex =  new Error (ex);
        }

        /// <summary>
        /// Конструктор по строке
        /// </summary>
        /// <param name="s">Сообщение</param>
        public Result(string s, Guid id) : this(id)
        {
            Value = null;
            Valid = false;
            Message = s;
            Ex = null;
        }
    }
}