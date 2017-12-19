using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfConsole
{
    /// <summary>
    /// WCF-сервис
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfService : IWcfService
    {
        /// <summary>
        /// Идентификатор сервиса
        /// </summary>
        private Guid id;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public WcfService()
        {
            id = Guid.NewGuid();
        }

        /// <summary>
        /// Вычисление факториала
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Result Factorial(string s)
        {
            try
            {
                int f; // да, тут int
                f = int.Parse(s);
                if (f < 0)
                {
                    return new Result("Для отрицательных чисел факториал не определен", id);
                }

                long n = 1;
                for (int i = 1; i <= f; i++)
                {
                    long prev = n;
                    n *= i;
                    if (n < prev)
                    {
                        return new Result("Переполнение", id);
                    }
                }
                return new Result(n, id);
            }
            catch (Exception ex)
            {
                return new Result(ex, id);
            }
        }
    }
}
