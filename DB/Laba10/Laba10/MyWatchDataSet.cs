using System;

namespace Laba10
{


    partial class MyWatchDataSet
    {
        partial class CompanyDataTable
        {
            protected override void OnColumnChanged(System.Data.DataColumnChangeEventArgs e)
            {
                CompanyRow row = (CompanyRow)e.Row;

                if (row.CompAdminMID >20)
                {
                    row.SetColumnError(e.Column, @"Can be only < 20");
                    row.RowError = @"Errors have occurred";
                }
                else
                    row.ClearErrors();

            }
        }

        partial class ManagerDataTable
        {
            protected override void OnColumnChanged(System.Data.DataColumnChangeEventArgs e)
            {
                ManagerRow row= (ManagerRow)e.Row;


                if (row.PhoneNumber.Length != 11)
                {
                    row.SetColumnError(e.Column, @"Not NULL");
                    row.RowError = @"Errors have occurred";
                    return;
                }
                row.ClearErrors();

                if (row.CompID >20)
                {
                    row.SetColumnError(e.Column, @"Can be only <= 20");
                    row.RowError = @"Errors have occurred";
                }
                else
                    row.ClearErrors();

            }
        }

        partial class VisitorDataTable
        {
            protected override void OnColumnChanged(System.Data.DataColumnChangeEventArgs e)
            {
                VisitorRow row = (VisitorRow)e.Row;


                if (row.PhoneNumber.Length != 11)
                {
                    row.SetColumnError(e.Column, @"Not NULL");
                    row.RowError = @"Errors have occurred";
                    return;
                }
                row.ClearErrors();

                if (!row.Email.Contains("@"))
                {
                    row.SetColumnError(e.Column, @"must have @");
                    row.RowError = @"Errors have occurred";
                }
                else
                    row.ClearErrors();

            }
        }

        partial class  MeetingDataTable
        {
            protected override void OnColumnChanged(System.Data.DataColumnChangeEventArgs e)
            {
                MeetingRow row = (MeetingRow)e.Row;


                if (row.TimeStart > row.TimeEnd)
                {
                    row.SetColumnError(e.Column, @"Bad Date");
                    row.RowError = @"Errors have occurred";
                    return;
                }
                row.ClearErrors();
                

            }
        }
    }
}
