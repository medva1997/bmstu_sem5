using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROC.EDUACATION.CSharpBotService.Storage
{
    /// <summary>
    /// Журнал регистрации 
    /// </summary>
    public class BotLog
    {
        public Guid ID { get; set; }
        public string username { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public int UserUD { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime dateTime { get; set; }
    }
}
