using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SoapService
{
    /// <summary>
    /// Веб-сервис ASP.NET, SOAP
    /// </summary>
    [WebService(Namespace = "http://www.orioner.ru")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FirstService : System.Web.Services.WebService
    {

        /// <summary>
        /// Привет всем!
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string HelloWorld()
        {
            return "Доброе утречко!";
        }

        /// <summary>
        /// Факториал f!
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [WebMethod(Description ="Вычисление факториала")]
        public Result Factorial(string s)
        {
            try
            {
                int f; // да, тут int
                f = int.Parse(s);
                if (f < 0)
                {
                    return null;
                }

                long n = 1;
                for (int i = 1; i <= f; i++)
                {
                    long prev = n;
                    n *= i;
                    if (n < prev)
                    {
                        return new Result("Переполнение");
                    }
                }
                return new Result (n);
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}
