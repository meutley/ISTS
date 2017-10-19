using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using ISTS.Domain.Rooms;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.Studios
{
    public class Studio : IAggregateRoot
    {
        private List<Room> _rooms = new List<Room>();

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string FriendlyUrl { get; private set; }

        public virtual ReadOnlyCollection<Room> Rooms
        {
            get { return _rooms.AsReadOnly(); }
            private set { _rooms = value.ToList(); }
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
            _rooms.Add(room);

            return room;
        }
    }
}