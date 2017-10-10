using System;

namespace ISTS.Helpers.Validation
{
    public static class ArgumentNotNullValidator
    {
        public static void Validate(object argument, string argumentName)
        {
            ArgumentValidator.Validate(ValidationType.NotNull, argument, argumentName);
        }
    }
}