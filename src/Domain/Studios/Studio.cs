using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Rooms;
using ISTS.Helpers.Async;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.Studios
{
    public class Studio : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string FriendlyUrl { get; protected set; }

        public virtual ICollection<Room> Rooms { get; set; }

        public static Studio Create(string name, string friendlyUrl, IStudioValidator studioValidator)
        {
            ArgumentNotNullValidator.Validate(name, nameof(name));
            ArgumentNotNullValidator.Validate(friendlyUrl, nameof(friendlyUrl));

            var validationResult = AsyncHelper.RunSync(() => studioValidator.ValidateAsync(null, name, friendlyUrl));
            if (validationResult == StudioValidatorResult.Success)
            {
                var result = new Studio
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    FriendlyUrl = friendlyUrl,
                    Rooms = new List<Room>()
                };

                return result;
            }

            throw new InvalidOperationException();
        }

        public void Update(string name, string friendlyUrl, IStudioValidator studioValidator)
        {
            ArgumentNotNullValidator.Validate(name, nameof(name));
            ArgumentNotNullValidator.Validate(friendlyUrl, nameof(friendlyUrl));

            var validationResult = AsyncHelper.RunSync(() => studioValidator.ValidateAsync(this.Id, name, friendlyUrl));
            if (validationResult == StudioValidatorResult.Success)
            {
                this.Name = name;
                this.FriendlyUrl = friendlyUrl;
            }
        }

        public Room CreateRoom(string name)
        {
            ArgumentNotNullValidator.Validate(name, nameof(name));

            var room = Room.Create(this.Id, name);
            Rooms.Add(room);

            return room;
        }
    }
}