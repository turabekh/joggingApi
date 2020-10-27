using System;
using System.Collections.Generic;
using System.Text;

namespace Models.JoggingModels
{
    public class WeekSummary
    {
        public int UserId { get; set; }
        public int WeekNumber { get; set; }
        public double AvgTimeInMinutes { get; set; }
        public double AvgSpeedMeterPerMinute { get; set; }
        public double AvgDistanceInMeters { get; set; }
        public IEnumerable<DateTime> WeekDates { get; set; }
    }
}
