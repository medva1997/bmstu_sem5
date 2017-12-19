using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfConsole
{
    public class Error
    {
        public string Message;

        public string StackTrace;

        public string Source;

        public Error InnerError;

        public Error()
        { }

        public Error(Exception ex)
        {
            Message = ex.Message;
            StackTrace = ex.StackTrace;
            Source = ex.Source;
            if (ex.InnerException == null)
            {
                InnerError = null;
            }
            else
            {
                InnerError = new Error(ex.InnerException);
            }
        }
    }
}
