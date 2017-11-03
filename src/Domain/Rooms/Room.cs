using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ISTS.Domain.Common;
using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;
using ISTS.Helpers.Async;

namespace ISTS.Domain.Rooms
{
    public class Room : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public string Name { get; protected set; }

        public virtual Studio Studio { get; protected set; }

        public virtual ICollection<Session> Sessions { get; set; }

        public static Room Create(Guid studioId, string name)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                StudioId = studioId,
                Name = name,
                Sessions = new List<Session>()
            };

            return room;
        }

        public Session CreateSession(DateRange scheduledTime, ISessionScheduleValidator sessionScheduleValidator)
        {
            var result = Room.ValidateSchedule(this.Id, null, scheduledTime, sessionScheduleValidator);
            if (result)
            {
                var session = Session.Create(this.Id, scheduledTime);
                Sessions.Add(session);
                return session; 
            }

            throw new InvalidOperationException();
        }

        public Session RescheduleSession(Session session, DateRange newSchedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            Session result = null;
            DoWithSession(session.Id, (s) =>
            {
                var validatorResult = Room.ValidateSchedule(this.Id, session.Id, newSchedule, sessionScheduleValidator);
                if (validatorResult)
                {
                    var sessionModel = s.Reschedule(newSchedule);
                    result = sessionModel;
                }
            });

            return result;
        }

        public Session StartSession(Guid sessionId, DateTime time)
        {
            Session result = null;
            DoWithSession(sessionId, (session) =>
            {
                if (session.ActualEndTime.HasValue)
                {
                    throw new SessionAlreadyEndedException();
                }
                
                if (session.ActualStartTime.HasValue)
                {
                    throw new SessionAlreadyStartedException();
                }
                
                result = session.SetActualStartTime(time);
            });

            return result;
        }

        public Session EndSession(Guid sessionId, DateTime time)
        {
            Session result = null;
            DoWithSession(sessionId, (session) =>
            {
                if (session.ActualEndTime.HasValue)
                {
                    throw new SessionAlreadyEndedException();
                }
                
                if (!session.ActualStartTime.HasValue)
                {
                    throw new SessionNotStartedException();
                }
                
                result = session.SetActualEndTime(time);
            });

            return result;
        }

        public Session ResetActualTime(Guid sessionId)
        {
            Session result = null;
            DoWithSession(sessionId, (session) =>
            {
                result = session.ResetActualTime();
            });

            return result;
        }

        private void DoWithSession(Guid sessionId, Action<Session> action)
        {
            var session = Sessions.SingleOrDefault(s => s.Id == sessionId);
            if (session == null)
            {
                throw new ArgumentException($"Session Id {sessionId} not found in Room {this.Id}");
            }

            action(session);
        }

        private static bool ValidateSchedule(Guid roomId, Guid? sessionId, DateRange schedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            var validatorResult = AsyncHelper.RunSync(() => sessionScheduleValidator.ValidateAsync(roomId, sessionId, schedule));
            if (validatorResult != SessionScheduleValidatorResult.Success)
            {
                ScheduleValidatorHelper.HandleSessionScheduleValidatorError(validatorResult);
                return false;
            }
            
            return true;
        }
    }
}