using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


namespace HandWrittenUDF
{
    public class UserDefinedFunctions
    {
        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlInt32 GetRandomNumber()
        {
            Random rnd = new Random();
            return rnd.Next();
        }


        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlInt32 GetDate(SqlDateTime timeStart, SqlDateTime timeEnd)
        {
            TimeSpan temp = ((DateTime)timeEnd) - ((DateTime)timeStart);
            return (SqlInt32)(temp.TotalMinutes);
        }
    }

}
