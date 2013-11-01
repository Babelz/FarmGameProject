using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmi.Calendar
{
    public static class TimeSpanUtil
    {
        public static double ConvertMillisToMinutes(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalMinutes;
        }
        public static double ConvertSecondsToMinutes(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).TotalMinutes;
        }
        public static double ConverSecondsToHours(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).TotalHours;
        }
    }
}
