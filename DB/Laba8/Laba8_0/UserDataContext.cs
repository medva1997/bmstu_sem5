using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Laba8_0
{
    public class UserDataContext
    {
        public class UserDataContext1 : DataContext
        {
            public UserDataContext1(string connectionString)
                : base(connectionString)
            {

            }
           
            [Function(Name = "GetIndexRange")]
            [return: Parameter(DbType = "Int")]
            public int GetIndexRange(
                [Parameter(Name = "min", DbType = "Int")] ref int minIndex,
                [Parameter(Name = "max", DbType = "Int")] ref int maxIndex)
            {
                IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), minIndex, maxIndex);
                minIndex = ((int)(result.GetParameterValue(0)));
                maxIndex = ((int)(result.GetParameterValue(1)));
                return ((int)(result.ReturnValue));
            }
        }
    }
}
