using System;

using Xunit;

using ISTS.Helpers.Validation;

namespace ISTS.Helpers.Test.Validation
{
    public class ArgumentValidatorTests
    {
        [Fact]
        public void Validate_Performs_Null_Check()
        {
            string argument = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => ArgumentValidator.Validate(ValidationType.NotNull, argument, nameof(argument)));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Does_Not_Throw_Exception_When_Argument_Not_Null()
        {
            string argument = "Test";
            var result = false;

            try
            {
                ArgumentValidator.Validate(ValidationType.NotNull, argument, nameof(argument));
                result = true;
            }
            catch (System.Exception)
            {
                throw;
            }

            Assert.True(result);
        }
    }
}