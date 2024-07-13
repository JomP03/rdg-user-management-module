using Domain.Entities.Users;
using Domain.Shared;

namespace Unit.Tests.Domain.UserTests
{
    [Collection("UserPhoneNumberTests")]
    public class UserPhoneNumberTests
    {

        [Theory]
        [InlineData("913756104")]
        [InlineData("351933756104")]
        [InlineData("923756104")]
        [InlineData("933756104")]
        [InlineData("963756104")]
        [InlineData("351963756104")]
        public void Constructor_ValidCases_ShouldCreateUserPhoneNumber(string validPhoneNumber)
        {
            // Act
            UserPhoneNumber userPhoneNumber = new(validPhoneNumber);

            // Assert
            Assert.Equal(validPhoneNumber, userPhoneNumber.Value);
        }

        [Fact]
        public void Constructor_NullPhoneNumber_ShouldThrowArgumentNullException()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UserPhoneNumber(null));
        }

        [Theory]
        [InlineData("91562478")] // Does not have valid size 
        [InlineData("invalid")] // Contains letters
        [InlineData("914012 231")] // Contains spaces
        [InlineData("941567325")] // Does not match the valid prefixes
        public void Constructor_InvalidCases_ShouldThrowBusinessRuleValidationException(string invalidPhoneNumber)
        {
            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => new UserPhoneNumber(invalidPhoneNumber));
        }
    }
}
