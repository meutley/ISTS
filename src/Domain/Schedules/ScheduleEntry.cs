using System;

namespace ISTS.Domain.Schedules
{
    public abstract class ScheduleEntry : IDomainObject
    {
        public Guid Id { get; protected set; }

        public DateTime StartDateTime { get; protected set; }

        public DateTime EndDateTime { get; protected set; }

        public virtual void Reschedule(DateTime newStartDateTime, DateTime newEndDateTime)
        {
            this.StartDateTime = newStartDateTime;
            this.EndDateTime = newEndDateTime;
        }
    }
}