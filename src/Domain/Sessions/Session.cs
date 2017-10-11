using System;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Sessions
{
    public class Session : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public DateRange ScheduledTime { get; protected set; }

        public static Session Create(Guid studioId, DateRange schedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            if (ValidateSchedule(studioId, null, schedule, sessionScheduleValidator))
            {
                var session = new Session
                {
                    StudioId = studioId,
                    ScheduledTime = schedule
                };

                return session;
            }

            throw new InvalidOperationException();
        }

        public Session Reschedule(DateRange newSchedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            if (Session.ValidateSchedule(this.StudioId, this.Id, newSchedule, sessionScheduleValidator))
            {
                this.ScheduledTime = newSchedule;
                return this;
            }

            throw new InvalidOperationException();
        }

        private static bool ValidateSchedule(Guid studioId, Guid? sessionId, DateRange schedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            var validatorResult = sessionScheduleValidator.Validate(studioId, sessionId, schedule);
            if (validatorResult != SessionScheduleValidatorResult.Success)
            {
                ScheduleValidatorHelper.HandleSessionScheduleValidatorError(validatorResult);
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}