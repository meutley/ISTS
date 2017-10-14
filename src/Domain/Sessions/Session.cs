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
            if (ValidateSchedule(roomId, null, schedule, sessionScheduleValidator))
            {
                var session = new Session
                {
                    RoomId = roomId,
                    ScheduledTime = schedule
                };

                return session;
            }

            throw new InvalidOperationException();
        }

        public Session Reschedule(DateRange newSchedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            if (Session.ValidateSchedule(this.RoomId, this.Id, newSchedule, sessionScheduleValidator))
            {
                this.ScheduledTime = newSchedule;
                return this;
            }

            throw new InvalidOperationException();
        }

        private static bool ValidateSchedule(Guid roomId, Guid? sessionId, DateRange schedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            var validatorResult = sessionScheduleValidator.Validate(roomId, sessionId, schedule);
            if (validatorResult != SessionScheduleValidatorResult.Success)
            {
                ScheduleValidatorHelper.HandleSessionScheduleValidatorError(validatorResult);
            }
            
            return validatorResult == SessionScheduleValidatorResult.Success;
        }
    }
}