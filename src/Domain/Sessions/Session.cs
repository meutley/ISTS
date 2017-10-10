using System;

using ISTS.Domain.Studios;

namespace ISTS.Domain.Sessions
{
    public class Session : IDomainObject
    {
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public static Session Create(Studio studio)
        {
            if (studio == null)
            {
                throw new ArgumentNullException(nameof(studio));
            }

            var result = new Session
            {
                Id = Guid.NewGuid(),
                StudioId = studio.Id
            };

            return result;
        }
    }
}