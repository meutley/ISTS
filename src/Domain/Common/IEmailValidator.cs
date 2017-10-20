using System;

namespace ISTS.Domain.Common
{
    public interface IEmailValidator
    {
        void Validate(string email);
    }
}