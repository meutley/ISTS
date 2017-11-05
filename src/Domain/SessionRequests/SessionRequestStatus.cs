using System;

namespace ISTS.Domain.SessionRequests
{
    public class SessionRequestStatus
    {
        public int Id { get; protected set; }

        public string Description { get; protected set; }

        public static SessionRequestStatus Create(string description)
        {
            return new SessionRequestStatus
            {
                Description = description
            };
        }
    }
}