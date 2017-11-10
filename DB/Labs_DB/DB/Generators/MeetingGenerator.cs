using System;

using DB.Tables;

namespace DB.Generators
{
    class MeetingGenerator:BaseGenerator<Meeting>
    {
        private void DateGenerator(WorkHours wh, out DateTime timeStart,out DateTime timeEnd)
        {

            
            int days = (wh.EventEnd - wh.EventStart).Days;
            DateTime meeting=wh.EventStart.AddDays(Rnd.Next(days+1));

            meeting=meeting.Add(wh.DayStart);
            TimeSpan ww = wh.DayEnd - wh.DayStart;
            int minutes = Convert.ToInt32(ww.TotalMinutes / 15);
            int st = Rnd.Next(minutes-2);
            int end = Rnd.Next(st, minutes);

            //время начала встречи
            timeStart = meeting.AddMinutes(st*15);
            //время конца встречи
            timeEnd = meeting.AddMinutes(end*15);





        }

        

        public MeetingGenerator(int managers, int visitors,int meetings, WorkHours wh, BusyGenerator man, BusyGenerator vis)
        {
            for (int i = 0; i < meetings;)
            {
                DateTime timeStart, timeEnd;
                DateGenerator(wh, out timeStart, out timeEnd);
                int mid = Rnd.Next(managers) + 1;
                int vid = Rnd.Next(visitors) + 1;
                bool res1 = man.CheckFree(mid, timeStart, timeEnd);
                bool res2 = vis.CheckFree(vid, timeStart, timeEnd);

                if (res2 && res1)
                {
                    man.MarkAsBusy(mid, timeStart, timeEnd);
                    vis.MarkAsBusy(vid, timeStart, timeEnd);
                    i++;
                    Lst.Add(new Meeting()
                    {Location = "Room "+i,
                    MConfirmation = Rnd.Next(0,2),
                    VConfirmation = Rnd.Next(0, 2),
                    Mid = mid,
                    Vid = vid,
                    TimeStart = timeStart,
                    TimeEnd = timeEnd,
                    MeetId = i+1
                    
                        
                    });
                }
               
            }
            
        }
    }
}
