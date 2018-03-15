using System;
using System.Globalization;

namespace YouTrackClientVS.Infrastructure.Utils
{
    public static class TimeConverter
    {
        public static DateTime GetDate(string date)
        {
            if (string.IsNullOrEmpty(date))
                return DateTime.MaxValue;

            return DateTime.Parse(date, CultureInfo.InvariantCulture).ToLocalTime();
        }
    }
}
