using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CROC.EDUACATION.CSharpBotService
{
    partial class BotService : ServiceBase
    {
        private CSharpBot bot;
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public BotService()
        {
            InitializeComponent();
            bot= new CSharpBot();
        }

        /// <summary>
        /// События запуска сервиса
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                bot.Start();
                EventLog.WriteEntry("Сервис успешно запущен", EventLogEntryType.Information, (int)EventId.Start);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(e.Message, EventLogEntryType.Information,(int)EventId.Exeption);
                throw;
            }
            
        }

        /// <summary>
        /// событие останова
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                bot.Stop();
                EventLog.WriteEntry("Сервис успешно остановлен", EventLogEntryType.Information, (int)EventId.Stop);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(e.Message, EventLogEntryType.Information, (int)EventId.Exeption);
                throw;
            }
        }

        /// <summary>
        /// Событие приостонова сервиса
        /// </summary>
        protected override void OnPause()
        {
            try
            {
                EventLog.WriteEntry("Сервис поставлен на паузу", EventLogEntryType.Information, (int)EventId.Pause);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(e.Message, EventLogEntryType.Information, (int)EventId.Exeption);
                throw;
            }
        }

        /// <summary>
        /// событие возобновления сервиса
        /// </summary>
        protected override void OnContinue()
        {
            try 
            {
                EventLog.WriteEntry("Сервис успешно возобновлен", EventLogEntryType.Information, (int)EventId.Continue);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(e.Message, EventLogEntryType.Information, (int)EventId.Exeption);
                throw;
            }
        }
    }
}
