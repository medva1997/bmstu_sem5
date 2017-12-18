using System;
using System.Collections.Generic;

using DB.Tables;

namespace DB.Generators
{
    class BusyGenerator:BaseGenerator<Busy>
    {
        /// <summary>
        /// Первый день работы
        /// </summary>
        private readonly DateTime _eventStart;
        /// <summary>
        /// последний день работы
        /// </summary>
        private readonly DateTime _eventEnd;
        /// <summary>
        /// Начало рабочего дня
        /// </summary>
        private readonly TimeSpan _dayStart;
        /// <summary>
        /// Конец рабочего дня
        /// </summary>
        private readonly TimeSpan _dayEnd;
        /// <summary>
        /// Шаг для генерарации
        /// </summary>
        private readonly TimeSpan _step; 

        /// <summary>
        /// Список рабочих слотов
        /// </summary>
        private readonly List<DateTime> _slotsList =new List<DateTime>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wh"> Часы работы</param>
        /// <param name="nUsers">Количество пользователей</param>
        public BusyGenerator(WorkHours wh, int nUsers)
        {
            _eventStart = wh.EventStart;
            _eventEnd = wh.EventEnd;
            _dayStart = wh.DayStart;
            _dayEnd = wh.DayEnd;
            _step = wh.Step;
            //генерация списка слотов
            SlotsGenerator();
            CreateTable(nUsers);

        }

        /// <summary>
        /// создание таблицы занятости пользователей
        /// </summary>
        /// <param name="nUsers"></param>
        private void CreateTable(int nUsers)
        {
            for (int i = 0; i < nUsers; i++)
            {
                foreach (DateTime time in _slotsList)
                {
                    Lst.Add(new Busy(){Uid = i+1, Time = time,Status = 0});
                }
            }
        }

        /// <summary>
        /// Отметить занятость пользователя в это время
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        public void MarkAsBusy(int uid, DateTime timeStart, DateTime timeEnd)
        {
            foreach (Busy busy in Lst)
            {
                if (busy.Uid == uid)
                {
                    if (busy.Time>=timeStart && busy.Time<timeEnd)
                    {
                        busy.Status = 1;
                    }
                }
            }
        }

        public bool CheckFree(int uid, DateTime timeStart, DateTime timeEnd)
        {
           
            foreach (Busy busy in Lst)
            {
                if (busy.Uid == uid)
                {
                    if (busy.Time >= timeStart && busy.Time < timeEnd)
                    {
                        if (busy.Status==1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// Вывод на экран слотов
        /// </summary>
        public void PrintSlots()
        {
            foreach (DateTime time in _slotsList)
            {
                Console.WriteLine(time);
            }
        }

        /// <summary>
        /// Генератор слотов
        /// </summary>
        private void SlotsGenerator()
        {
            for (DateTime date = _eventStart; date <= _eventEnd; date = date.AddDays(1))
            {
                TimeSpan time = _dayStart;
               
                for (; time<_dayEnd; time+=_step)
                {
                    _slotsList.Add(new DateTime(date.Year,date.Month,date.Day,time.Hours,time.Minutes,time.Seconds));
                }
            }
            
        }
        

    }
}
