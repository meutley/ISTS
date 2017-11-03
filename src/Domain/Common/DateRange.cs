using System;

namespace ISTS.Domain.Common
{
    public class DateRange : ValueObject<DateRange>
    {
        public DateTime Start { get; protected set; }

        public DateTime End { get; protected set; }

        public static DateRange Create(DateTime start, DateTime end)
        {
            return new DateRange
            {
                Start = start,
                End = end
            };
        }

        protected override bool EqualsCore(DateRange other) =>
            Start == other.Start
            && End == other.End;

        public override int GetHashCodeCore()
        {
            unchecked
            {
                int hash = 397;
                hash = hash * 397 + (int)Start.Ticks;
                hash = hash * 397 + (int)End.Ticks;
                return hash;
            }
        }
    }
}