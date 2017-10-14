using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ISTS.Helpers.Validation;

namespace ISTS.Domain.Studios
{
    public class Studio : IAggregateRoot
    {
        private List<StudioRoom> _rooms = new List<StudioRoom>();

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string FriendlyUrl { get; private set; }

        public virtual ReadOnlyCollection<StudioRoom> StudioRooms
        {
            get { return _rooms.AsReadOnly(); }
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

        public StudioRoom CreateRoom(string name)
        {
            ArgumentNotNullValidator.Validate(name, nameof(name));

            var room = StudioRoom.Create(this.Id, name);
            _rooms.Add(room);

            return room;
        }
    }
}