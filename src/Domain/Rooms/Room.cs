using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Domain.Rooms
{
    public class Room : IAggregateRoot
    {
        private List<RoomSession> _sessions = new List<RoomSession>();
        
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public string Name { get; protected set; }

        public virtual Studio Studio { get; protected set; }

        public ReadOnlyCollection<RoomSession> Sessions
        {
            get { return _sessions.AsReadOnly(); }
        }

        public static Room Create(Guid studioId, string name)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                StudioId = studioId,
                Name = name
            };

            return room;
        }

        public RoomSession CreateSession(DateRange scheduledTime, ISessionScheduleValidator sessionScheduleValidator)
        {
            var validatorTask = Room.ValidateSchedule(this.Id, null, scheduledTime, sessionScheduleValidator);
            Task.WaitAll(validatorTask);
            if (validatorTask.Result)
            {
                var session = RoomSession.Create(this.Id, scheduledTime);
                _sessions.Add(session);
                return session; 
            }

            throw new InvalidOperationException();
        }

        public RoomSession RescheduleSession(RoomSession session, DateRange newSchedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            RoomSession result = null;
            DoWithSession(session.Id, (s) =>
            {
                var validatorTask = Room.ValidateSchedule(this.Id, session.Id, newSchedule, sessionScheduleValidator);
                Task.WaitAll(validatorTask);
                if (validatorTask.Result)
                {
                    var sessionModel = s.Reschedule(newSchedule);
                    result = sessionModel;
                }
            });

            return result;
        }

        public RoomSession StartSession(Guid sessionId, DateTime time)
        {
            RoomSession result = null;
            DoWithSession(sessionId, (session) =>
            {
                if (session.ActualStartTime.HasValue)
                {
                    throw new SessionAlreadyStartedException();
                }
                
                result = session.SetActualStartTime(time);
            });

            return result;
        }

        public RoomSession EndSession(Guid sessionId, DateTime time)
        {
            RoomSession result = null;
            DoWithSession(sessionId, (session) =>
            {
                if (!session.ActualStartTime.HasValue)
                {
                    throw new SessionNotStartedException();
                }
                
                result = session.SetActualEndTime(time);
            });

            return result;
        }

        public RoomSession ResetActualTime(Guid sessionId)
        {
            RoomSession result = null;
            DoWithSession(sessionId, (session) =>
            {
                result = session.ResetActualTime();
            });

            return result;
        }

        private void DoWithSession(Guid sessionId, Action<RoomSession> action)
        {
            var session = _sessions.SingleOrDefault(s => s.Id == sessionId);
            if (session == null)
            {
                throw new ArgumentException($"Session Id {sessionId} not found in Room {this.Id}");
            }

            action(session);
        }

        private async static Task<bool> ValidateSchedule(Guid roomId, Guid? sessionId, DateRange schedule, ISessionScheduleValidator sessionScheduleValidator)
        {
            var validatorResult = await sessionScheduleValidator.ValidateAsync(roomId, sessionId, schedule);
            if (validatorResult != SessionScheduleValidatorResult.Success)
            {
                ScheduleValidatorHelper.HandleSessionScheduleValidatorError(validatorResult);
                return false;
            }
            
            return true;
        }
    }
}