using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.Common
{
    public record DateTimeRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public DateTimeRange(DateTime start, DateTime end)
        {
            if (end < start)
                throw new ArgumentException("End date must be greater than or equal to start date", nameof(start));

            Start = start;
            End = end;
        }

        public bool Contains(DateTime dateTime)
        {
            return dateTime >= Start && dateTime <= End;
        }

        public bool Overlaps(DateTimeRange other)
        {
            return Start <= other.End && End >= other.Start;
        }

        public TimeSpan Duration => End - Start;

        public bool IsExpired => End < DateTime.UtcNow;

        public static DateTimeRange Create(DateTime start, TimeSpan duration)
        {
            return new DateTimeRange(start, start.Add(duration));
        }

        public static DateTimeRange Today => new(DateTime.Today, DateTime.Today.AddDays(1).AddTicks(-1));

        public static DateTimeRange ThisWeek => new(
            DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek),
            DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek).AddTicks(-1)
        );

        public static DateTimeRange ThisMonth => new(
            new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
            new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddTicks(-1)
        );
    }
}
