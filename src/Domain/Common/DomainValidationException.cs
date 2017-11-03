using System;

namespace ISTS.Domain.Common
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(Exception innerException)
            : base("Data validation error. See inner exception for details.", innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException(nameof(innerException));
            }
        }
    }
}