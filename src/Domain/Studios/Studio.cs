using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.Studios
{
    public class Studio : IAggregateRoot
    {
        private List<StudioSession> _studioSessions = new List<StudioSession>();

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string FriendlyUrl { get; private set; }

        public virtual ReadOnlyCollection<StudioSession> StudioSessions
        {
            get { return _studioSessions.AsReadOnly(); }
        }

        public static Studio Create(string name, string friendlyUrl)
        {
            ArgumentNotNullValidator.Validate(name, nameof(name));
            ArgumentNotNullValidator.Validate(friendlyUrl, nameof(friendlyUrl));

            var result = new Studio
            {
                Id = Guid.NewGuid(),
                Name = name,
                FriendlyUrl = friendlyUrl
            };

            return result;
        }

        public StudioSession CreateSession(DateRange scheduledTime, ISessionScheduleValidator sessionScheduleValidator)
        {
            var validatorResult = sessionScheduleValidator.Validate(this.Id, null, scheduledTime);
            if (validatorResult != SessionScheduleValidatorResult.Success)
            {
                ScheduleValidatorHelper.HandleSessionScheduleValidatorError(validatorResult);
            }
            else
            {
                var session = StudioSession.Create(this.Id, scheduledTime);
                return session;
            }

            throw new InvalidOperationException();
        }
    }
}