using System;
using System.Net.Mail;

using ISTS.Domain.Common;

namespace ISTS.Application.Common
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
                throw new DataValidationException(new FormatException(string.Format("The email address is not in a recognized format: {0}", email)));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}