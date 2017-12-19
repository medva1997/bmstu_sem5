using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROC.EDUACATION.CSharpBotService
{
    public enum BotStates
    {
        None,
        RegistrationEmail,
        RegistrationPhone,
        Checkin
    }

    public enum EventId
    {
        None,
        Start,
        Stop,
        Pause,
        Continue,
        Exeption,
    }

    /// <summary>
    /// Сеанс работы бота
    /// </summary>
    class BotSession
    {
        //состояние
        public BotStates state { get; set; }
        /// <summary>
        ///  метка времени
        /// </summary>
        public DateTime TimeStamp { get; set; }

        public long ChatId { get; set; }

        public BotSession()
        {

            TimeStamp=DateTime.Now;
            
        }
    }
}
