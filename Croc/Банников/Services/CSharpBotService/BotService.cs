using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CROC.Education.CSharpBotService
{
    /// <summary>
    /// Служба операционной системы
    /// </summary>
    partial class BotService : ServiceBase
    {
        /// <summary>
        /// Бот
        /// </summary>
        private CSharpBot bot;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public BotService()
        {
            InitializeComponent();
            bot = new CSharpBot();
        }

        /// <summary>
        /// Событие запуска сервиса
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                bot.Start();
                EventLog.WriteEntry("Сервис успешно запущен", EventLogEntryType.Information, (int)EventID.Start);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Information, (int)EventID.Exception);
            }
        }

        /// <summary>
        /// Событие останова сервиса
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                bot.Stop();
                EventLog.WriteEntry("Сервис успешно остановлен", EventLogEntryType.Information, (int)EventID.Stop);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Information, (int)EventID.Exception);
            }
        }

        /// <summary>
        /// Событие приостанова сервиса
        /// </summary>
        protected override void OnPause()
        {
            try
            {
                bot.Stop();
                EventLog.WriteEntry("Сервис успешно приостановлен", EventLogEntryType.Information, (int)EventID.Pause);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Information, (int)EventID.Exception);
            }
        }

        /// <summary>
        /// Событие возобновления сервиса
        /// </summary>
        protected override void OnContinue()
        {
            try
            {
                bot.Start();
                EventLog.WriteEntry("Сервис успешно возобновлен", EventLogEntryType.Information, (int)EventID.Continue);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Information, (int)EventID.Exception);
            }
        }
    }
}
