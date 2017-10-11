using System;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.Studios
{
    public class Studio : IAggregateRoot
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string FriendlyUrl { get; private set; }

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
            var validatorResult = sessionScheduleValidator.Validate(this.Id, scheduledTime);
            if (validatorResult != SessionScheduleValidatorResult.Success)
            {
                HandleSessionScheduleValidatorError(validatorResult);
            }
            else
            {
                var session = StudioSession.Create(this.Id, scheduledTime);
                return session;
            }

            throw new InvalidOperationException();
        }

        private void HandleSessionScheduleValidatorError(SessionScheduleValidatorResult validatorResult)
        {
            switch (validatorResult)
            {
                case SessionScheduleValidatorResult.Success:
                    return;
                case SessionScheduleValidatorResult.Overlapping:
                    throw new OverlappingScheduleException();
                case SessionScheduleValidatorResult.StartProvidedEndNull:
                case SessionScheduleValidatorResult.StartNullEndProvided:
                    throw new ScheduleStartAndEndMustBeProvidedException();
                case SessionScheduleValidatorResult.StartGreaterThanOrEqualToEnd:
                    throw new ScheduleEndMustBeGreaterThanStartException();
            }
        }
    }
}