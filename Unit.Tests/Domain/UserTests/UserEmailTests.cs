using Domain.Entities.Users;
using Domain.Shared;

namespace Unit.Tests.Domain.UserTests
{
    [Collection("UserEmailTests")]
    public class UserEmailTests
    {
        public UserEmailTests()
        {
            // Set up the environment variable for testing
            Environment.SetEnvironmentVariable("ALLOWED_DOMAINS", "isep.ipp.pt,gmail.com");
        }

        [Theory]
        [InlineData("test@gmail.com")]
        [InlineData("test@isep.ipp.pt")]
        [InlineData("some.other@isep.ipp.pt")]
        public void Constructor_ValidCases_ShouldCreateUserEmail(string validEmail)
        {
            // Act
            UserEmail userEmail = new(validEmail);

            // Assert
            Assert.Equal(validEmail, userEmail.Value);
        }

        [Fact]
        public void Constructor_NullEmail_ShouldThrowArgumentNullException()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UserEmail(null));
        }

        [Theory]
        [InlineData("invalid")] // Does not match the email format
        [InlineData("invalid@")] // Does not match the email format
        [InlineData("invalid@email")] // Does not match the email format
        [InlineData("invalid@domain.")] // Does not match the email format
        [InlineData("invalid@domain.com")] // Does not match the valid domains
        [InlineData("invalid@isep.pt")] // Does not match the valid domains
        public void Constructor_InvalidCases_ShouldThrowBusinessRuleValidationException(string invalidEmail)
        {
            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => new UserEmail(invalidEmail));
        }
    }
}
