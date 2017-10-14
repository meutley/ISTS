using System;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Sessions
{
    public class Session : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public Guid RoomId { get; protected set; }

        public DateRange ScheduledTime { get; protected set; }

        public static Session Create(Guid roomId, DateRange schedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            var session = new Session
            {
                RoomId = roomId,
                ScheduledTime = schedule
            };

            return session;
        }
    }
}