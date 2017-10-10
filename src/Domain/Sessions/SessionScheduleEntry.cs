using System;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Sessions
{
    public class SessionScheduleEntry : ScheduleEntry
    {
        public Guid SessionId { get; protected set; }

        public static SessionScheduleEntry Create(Session session, DateTime startDateTime, DateTime endDateTime)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            var result = new SessionScheduleEntry
            {
                Id = Guid.NewGuid(),
                SessionId = session.Id,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime
            };

            return result;
        }
    }
}