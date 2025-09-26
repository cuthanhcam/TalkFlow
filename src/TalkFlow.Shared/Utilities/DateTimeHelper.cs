using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Shared.Utilities
{
    public static class DateTimeHelper
    {
        public static DateTime UtcNow => DateTime.UtcNow;

        public static DateTime ToUtc(this DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
        }

        public static string ToIso8601String(this DateTime dateTime)
        {
            return dateTime.ToUtc().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static bool IsExpired(this DateTime dateTime)
        {
            return dateTime.ToUtc() < UtcNow;
        }

        public static TimeSpan TimeUntil(this DateTime dateTime)
        {
            return dateTime.ToUtc() - UtcNow;
        }
    }
}
