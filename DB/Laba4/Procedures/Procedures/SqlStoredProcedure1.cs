using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void GetInfoByManagerMettings(string mid)
    {
        using (SqlConnection contextConnection = new SqlConnection("context connection = true"))
        {
            SqlCommand contextCommand =
                new SqlCommand(
                    "SELECT Meet_ID, VID, TimeStart, TimeEnd FROM Meeting WHERE MID = @mid", contextConnection);

            contextCommand.Parameters.AddWithValue("@mid", mid);

            contextConnection.Open();

            SqlDataRecord record = new SqlDataRecord(
                new SqlMetaData("Meet_ID", SqlDbType.Int),
                new SqlMetaData("VID", SqlDbType.Int),
                new SqlMetaData("TimeStart", SqlDbType.DateTime),
                new SqlMetaData("TimeEnd", SqlDbType.DateTime)
            );

            SqlContext.Pipe.SendResultsStart(record);

            SqlDataReader reader = contextCommand.ExecuteReader();
            while (reader.Read())
            {
                record.SetInt32(0, reader.GetInt32(0));
                record.SetInt32(1, reader.GetInt32(1));
                record.SetDateTime(2, reader.GetDateTime(2));
                record.SetDateTime(3, reader.GetDateTime(3));

                SqlContext.Pipe.SendResultsRow(record);
            }

            SqlContext.Pipe.SendResultsEnd();
        }
    }
}
