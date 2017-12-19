using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROC.Education.CSharpBotService
{
    /// <summary>
    /// Типы событий сервиса
    /// </summary>
    enum EventID
    {
        None,
        /// <summary>
        /// Запуск сервиса
        /// </summary>
        Start,
        /// <summary>
        /// Останов сервиса
        /// </summary>
        Stop,
        /// <summary>
        /// Приостанов сервиса
        /// </summary>
        Pause,
        /// <summary>
        /// Возобновление сервиса
        /// </summary>
        Continue,
        /// <summary>
        /// Ошибка сервиса
        /// </summary>
        Exception
    }
}
