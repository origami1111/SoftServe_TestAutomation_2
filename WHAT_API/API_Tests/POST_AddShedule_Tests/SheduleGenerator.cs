using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHAT_API
{
    public class SheduleGenerator
    {
        private CreateSchedule schedule = new CreateSchedule();
        public CreateSchedule GenerateShedule(PatternType type, int interval,
                                              List<DayOfWeek> list, DateTime startDate, DateTime finishDate, int mentorID, int groupID, int themeID)
        {
            schedule.Pattern = new Pattern()
            {
                Type = type,
                Interval = interval,
                DaysOfWeek = list
            };

            schedule.Range = new OccurrenceRange()
            {
                StartDate = startDate,
                FinishDate = finishDate
            };

            schedule.Context = new Context()
            {
                MentorID = mentorID,
                ThemeID = groupID,
                GroupID = themeID
            };

            return schedule;
        }
    }
}