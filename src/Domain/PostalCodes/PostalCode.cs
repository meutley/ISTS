using System;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCode : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public string Code { get; protected set; }

        public static PostalCode Create(string code)
        {
            var result = new PostalCode
            {
                Id = Guid.NewGuid(),
                Code = code
            };

            return result;
        }
    }
}