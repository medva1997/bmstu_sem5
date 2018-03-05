using System;
using System.Collections.Generic;

namespace Laba6
{
    internal abstract class СonveyerBase<T>
    {
        /// <summary>
        /// Флаг отмены
        /// </summary>
        private bool _cancel;
        /// <summary>
        /// Флаг необходимости ожидания прошлого этапа
        /// </summary>
        protected bool Wait;

        /// <summary>
        /// Очередь с данными
        /// </summary>
        private readonly Queue<T> _queue;
        /// <summary>
        /// Следующий этап
        /// </summary>
        private СonveyerBase<T> _next;
        
        /// <summary>
        /// Блокировщик  
        /// </summary>
        private readonly object _locker=new object();

        /// <summary>
        /// Название потока
        /// </summary>
        protected string Name="";

        protected СonveyerBase()
        {
            _queue= new Queue<T>();
            _cancel = false;
            Wait = true;
        }

        /// <summary>
        /// Установка следующего этапа конвеера
        /// </summary>
        /// <param name="next"></param>
        public void SetNextСonveyer(СonveyerBase<T> next)
        {
            lock (_locker)
            {
                _next = next;
            }
        }

        /// <summary>
        /// Запуск выполнения работы
        /// </summary>
        public void Run()
        {
            
           
            
            Console.WriteLine($"{Name} :: Started");
            while (!_cancel)
            {
                T data;
                
                if (_queue.Count > 0)
                {
                    lock (_locker)
                    {
                        data = _queue.Dequeue();
                    }
                }
                else
                {
                    if (Wait==false)
                    {
                        Cancel();
                    }
                    continue;
                }
                Console.WriteLine($"{Name} <== {data}");
                data = Work(data);
                AddToNext(data);
                Console.WriteLine($"{Name} ==> {data}");
               
            }
            
            
        }

        /// <summary>
        /// Добавление данных в очередь для обработки
        /// </summary>
        /// <param name="data">данные</param>
        public void Enqueue(T data)
        {
            lock (_locker)
            {
                _queue.Enqueue(data);
            }
        }

        /// <summary>
        /// Передача данных в следущий этап обработки
        /// </summary>
        /// <param name="data">данные</param>
        private void AddToNext(T data)
        {
            _next?.Enqueue(data);
        }

        /// <summary>
        /// Остановка обработки
        /// </summary>
        private void Cancel()
        {
            _cancel = true;
            _queue.Clear();
            _next?.CancelNextWait();
            //Console.WriteLine($"{Name} :: Finished");
        }

        /// <summary>
        /// Отменена ожидания завершения
        /// </summary>
        private void CancelNextWait()
        {
            Wait = false;
        }

        /// <summary>
        /// Процедура обрабатываюящая данные
        /// </summary>
        /// <param name="data">входные данные</param>
        /// <returns>выходные данные</returns>
        protected abstract T Work(T data);

    }
}