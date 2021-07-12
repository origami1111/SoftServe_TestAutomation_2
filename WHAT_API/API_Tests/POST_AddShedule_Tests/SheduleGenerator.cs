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
        public CreateSchedule GenerateShedule()
        {
            CreateSchedule shedule = new CreateSchedule
            {
                Pattern = new Pattern
                {
                    Type = PatternType.Daily,
                    Interval = 3,
                    DaysOfWeek = { DayOfWeek.Monday, DayOfWeek.Friday }
                },

                Range = new OccurrenceRange
                {
                    StartDate = new DateTime(2020, 1, 3, 13, 27, 09).ToUniversalTime(),
                    FinishDate = new DateTime(2021, 7, 7, 15, 27, 09).ToUniversalTime()
                },

                Context = new Context
                {
                    MentorID = 1,
                    ThemeID = 4,
                    GroupID = 4
                }
            };

            return shedule;
        }
    }
}