using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Rooms;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.Studios
{
    public class Studio : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string FriendlyUrl { get; protected set; }

        public virtual ICollection<Room> Rooms { get; set; }

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

        public void Update(string name, string friendlyUrl)
        {
            ArgumentNotNullValidator.Validate(name, nameof(name));
            ArgumentNotNullValidator.Validate(friendlyUrl, nameof(friendlyUrl));

            this.Name = name;
            this.FriendlyUrl = friendlyUrl;
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