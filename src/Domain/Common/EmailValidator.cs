using System;
using System.Net.Mail;

namespace ISTS.Domain.Common
{
    public class EmailValidator : IEmailValidator
    {
        public void Validate(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new DomainValidationException(new FormatException(string.Format("The email address is not in a recognized format: {0}", email)));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}