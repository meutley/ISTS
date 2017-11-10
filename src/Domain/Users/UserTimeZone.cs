using System;
using System.Collections.Generic;

namespace ISTS.Domain.Users
{
    public class UserTimeZone
    {
        public int Id { get; protected set; }

        public string Name { get; protected set; }

        public Int16 UtcOffset { get; protected set; }

        public virtual ICollection<User> Users { get; set; }

        public static UserTimeZone Create(string name, Int16 utcOffset)
        {
            return new UserTimeZone
            {
                Name = name,
                UtcOffset = utcOffset
            };
        }
    }
}