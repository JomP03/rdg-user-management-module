using Domain.Entities.Users;
using Domain.Shared;

namespace Unit.Tests.Domain.UserTests
{
    [Collection("UserNifTests")]
    public class UserNifTests
    {
        // All generated using https://www.niffresquinho.com/ (Portuguese NIF generator)
        [Theory]
        [InlineData("172658462")] 
        [InlineData("252582241")]
        [InlineData("324954913")]
        [InlineData("453777279")]
        [InlineData("535813244")]
        [InlineData("679082433")]
        [InlineData("794576524")]
        [InlineData("980156122")]
        [InlineData("728121409")]
        [InlineData("757733689")]
        [InlineData("776897888")]
        [InlineData("805488200")]
        public void Constructor_ValidCases_ShouldCreateUserNif(string validNif)
        {
            // Act
            UserNif userNif = new(validNif);

            // Assert
            Assert.Equal(validNif, userNif.Value);
        }

        [Fact]
        public void Constructor_NullNif_ShouldThrowArgumentNullException()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UserNif(null));
        }

        [Theory]
        [InlineData("12345678")] // Has invalid size
        [InlineData("1234567890")] // Has invalid size
        [InlineData("12345678a")] // Contains letters
        [InlineData("12345678!")] // Contains symbols
        [InlineData("24398989 ")] // Contains spaces
        [InlineData("243989891")] // Does not meet the portguese validation algorithm
        public void Constructor_InvalidCases_ShouldThrowBusinessRuleValidationException(string invalidNif)
        {
            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => new UserNif(invalidNif));
        }
    }
}
