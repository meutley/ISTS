using System;

namespace ISTS.Domain.Studios
{
    public class StudioRoom
    {
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public virtual Studio Studio { get; protected set; }

        public string Name { get; protected set; }

        public static StudioRoom Create(Guid studioId, string name)
        {
            var studioRoom = new StudioRoom
            {
                Id = Guid.NewGuid(),
                StudioId = studioId,
                Name = name
            };

            return studioRoom;
        }
    }
}