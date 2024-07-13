using Domain.Entities.Users;

namespace Unit.Tests.Domain.UserTests
{
    [Collection("PasswordPolicyTests")]
    public class PasswordPolicyTests
    {
        [Theory]
        [InlineData("ValidPass123!")]
        [InlineData("AnotherValid123!")]
        [InlineData("123!ValidPass")]
        public void IsValidPassword_ValidCases_ReturnsTrue(string password)
        {
            // Act
            bool isValid = PasswordPolicy.IsValidPassword(password);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("invalidNoNumbers!")]
        [InlineData("invalidNoSymbols123")]
        [InlineData("invalidnouppercase123!")]
        [InlineData("INVALIDNOLOWERCASE123!")]
        public void IsValidPassword_InvalidCases_ReturnsFalse(string password)
        {
            // Act
            bool isValid = PasswordPolicy.IsValidPassword(password);

            // Assert
            Assert.False(isValid);
        }
    }
}
