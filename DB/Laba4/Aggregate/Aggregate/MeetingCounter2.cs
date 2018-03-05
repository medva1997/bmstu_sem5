using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.Native)]
public struct MeetingCounter2
{
    private int count;

    /// <summary>
    /// ??? ?????? ?????? ?????? ?????????? ???????? ????????? Init ?? ????????? ?????? ??????.
    /// </summary>
    public void Init()
    {

        count =0;
        // Put your code here
    }

    /// <summary>
    /// ??? ?????? ?????? ? ?????? ??????????? ???????? ????????? Accumulate
    /// </summary>
    /// <param name="Value"></param>
    public void Accumulate(SqlString Value)
    {

        count++;
        // Put your code here
    }

    public void Merge(MeetingCounter2 Group)
    {
        Accumulate(Group.ToString());
        // Put your code here
    }

    public override string ToString()
    {
        //count = 1;
        return count.ToString();
    }

    /// <summary>
    /// ????? ?????? ??????????, ??????????? ???????? ??????? Terminate.
    /// </summary>
    /// <returns></returns>
    public SqlString Terminate()
    {
        //count = 2;
        return count.ToString();
        //return new SqlString (string.Empty);
    }
}