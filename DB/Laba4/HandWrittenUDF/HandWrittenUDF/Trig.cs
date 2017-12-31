using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace HandWrittenUDF
{
    public class Trig
    {
        // Enter existing table or view for the target and uncomment the attribute line
        //[SqlTrigger(Name = "SqlTrigger1", Target = "[dbo].[Visitor]", Event = "FOR UPDATE")]
        public static void SqlTrigger1()
        {

            SqlTriggerContext triggerContext = SqlContext.TriggerContext;

            if (triggerContext.TriggerAction == TriggerAction.Update)
            {
                var connection = new SqlConnection("Context Connection=true");
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "SELECT PhoneNumber FROM INSERTED"
                };
                connection.Open();
                string phone = (string)command.ExecuteScalar();
                connection.Close();

                if (phone.Length != 11)
                {
                    SqlContext.Pipe.Send("Trigger FIRED");
                    throw new FormatException("Invalid phone");
                }
            }



        }
    }
}
