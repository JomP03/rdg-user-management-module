using Domain.Entities.Users;
using Domain.Shared;

namespace Unit.Tests.Domain.UserTests
{
    [Collection("UserPasswordTests")]
    public class UserPasswordTests
    {
        [Theory]
        [InlineData("ValidPass123!")]
        [InlineData("AnotherValid123!")]
        [InlineData("123!ValidPass")]
        public void Constructor_ValidCases_ShouldCreateUserPassword(string password)
        {
            // Act
            UserPassword userPassword = new(password);

            // Assert
            Assert.Equal(password, userPassword.Value);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("invalidNoNumbers!")]
        [InlineData("invalidNoSymbols123")]
        [InlineData("invalidnouppercase123!")]
        [InlineData("INVALIDNOLOWERCASE123!")]
        public void Constructor_InvalidCases_ShouldThrowBusinessRuleValidationException(string password)
        {
            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new UserPassword(password));
        }
    }
}
