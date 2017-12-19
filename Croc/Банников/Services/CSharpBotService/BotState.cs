using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROC.Education.CSharpBotService
{
    public enum BotState
    {
        /// <summary>
        /// Базовое состояние бота
        /// </summary>
        None,
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        Registration
    }
}
