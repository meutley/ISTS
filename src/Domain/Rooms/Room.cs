using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public class Room : IAggregateRoot
    {
        private List<RoomSession> _sessions = new List<RoomSession>();
        
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public string Name { get; protected set; }

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
            var validatorResult = sessionScheduleValidator.Validate(this.Id, null, scheduledTime);
            if (validatorResult != SessionScheduleValidatorResult.Success)
            {
                ScheduleValidatorHelper.HandleSessionScheduleValidatorError(validatorResult);
            }
            else
            {
                var session = RoomSession.Create(this.Id, scheduledTime);
                _sessions.Add(session);
                return session;
            }

            throw new InvalidOperationException();
        }
    }
}