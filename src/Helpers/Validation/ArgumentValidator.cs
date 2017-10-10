using System;

namespace ISTS.Helpers.Validation
{
    public static class ArgumentValidator
    {
        public static void Validate(ValidationType validationType, object argument, string argumentName)
        {
            switch (validationType)
            {
                case ValidationType.NotNull:
                    ValidateNotNull(argument, argumentName);
                    break;
            }
        }

        private static void ValidateNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }

    public enum ValidationType
    {
        NotNull
    }
}