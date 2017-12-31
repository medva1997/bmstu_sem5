using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(FillRowMethodName = "FillRow2", DataAccess = DataAccessKind.Read,
        TableDefinition = "Meet_ID int,VID int, TimeStart DateTime,TimeEnd DateTime", SystemDataAccess = SystemDataAccessKind.Read)]
    public static IEnumerable ManagerMeetings(SqlString mid)
    {
        using (SqlConnection connect = new SqlConnection("context connection=true"))
        {
            List<NameRow2> lst = new List<NameRow2>();
            SqlCommand cmd = new SqlCommand(
                "SELECT Meet_ID, VID, TimeStart, TimeEnd FROM Meeting WHERE MID = @mid", connect);
            cmd.Parameters.AddWithValue("@mid", mid);
            connect.Open();
            SqlDataReader blokReader = cmd.ExecuteReader();
            //int i = 0;
            while (blokReader.Read())
            {
                lst.Add(new NameRow2(blokReader.GetInt32(0), blokReader.GetInt32(1),
                    blokReader.GetDateTime(2),
                    blokReader.GetDateTime(3)));
                // i++;
            }
            connect.Close();
            return lst;
        }
    }

    public static void FillRow2(object row, out int Meet_ID, out int VID, out DateTime TimeStart,
        out DateTime TimeEnd)
    {
        // ?????? ?????? ?? ????????? ???????. 
        Meet_ID = ((NameRow2)row).Meet_ID;
        VID = ((NameRow2)row).VID;
        TimeStart = ((NameRow2)row).TimeStart;
        TimeEnd = ((NameRow2)row).TimeEnd;
    }
}
public class NameRow2
{
    public int Meet_ID;
    public int VID;
    public DateTime TimeStart;
    public DateTime TimeEnd;


    public NameRow2(int meetId, int vid, DateTime timeStart, DateTime timeEnd)
    {
        Meet_ID = meetId;
        VID = vid;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
    }
}
