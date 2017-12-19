using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpBotAdmin.Models
{
    /// <summary>
    /// Порядок сортировки
    /// </summary>
    public class SortOrder
    {
        /// <summary>
        /// Имя поля для сортировки
        /// </summary>
        public string Name;

        /// <summary>
        /// Признак сортировки в обратном направлении
        /// </summary>
        public bool Descent;      
    }
}