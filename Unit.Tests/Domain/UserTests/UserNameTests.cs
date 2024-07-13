using Domain.Entities.Users;
using Domain.Shared;

namespace Unit.Tests.Domain.UserTests
{
    [Collection("UserNameTests")]
    public class UserNameTests
    {
        public UserNameTests()
        {
            // Set up the environment variable for testing
            Environment.SetEnvironmentVariable("MAX_NAME_LENGTH", "20");
        }

        [Theory]
        [InlineData("ValidName")]
        [InlineData("AnotherValidName")]
        [InlineData("Valid Name")]
        public void Constructor_ValidCases_ShouldCreateUserName(string validName)
        {
            // Act
            UserName userName = new(validName);

            // Assert
            Assert.Equal(validName, userName.Value);
        }

        [Fact]
        public void Constructor_NullName_ShouldThrowArgumentNullException()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UserName(null));
        }

        [Theory]
        [InlineData("Invalid@")] // Is not only letters and spaces
        [InlineData("a")] // Is less than the minimum length
        [InlineData("ThisNameIsAboveTheMaximumLength123")] // Is more than the maximum length
        public void Constructor_InvalidCases_ShouldThrowBusinessRuleValidationException(string invalidName)
        {
            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => new UserName(invalidName));
        }
    }
}
