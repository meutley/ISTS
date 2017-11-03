using System;

namespace ISTS.Application.Common
{
    public class DataValidationException : Exception
    {
        public DataValidationException(Exception innerException)
            : base("Data validation error. See inner exception for details.", innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException(nameof(innerException));
            }
        }
    }
}