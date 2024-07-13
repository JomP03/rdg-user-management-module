using Domain.Shared;

namespace Unit.Tests.Domain.SharedTests
{
    [Collection("GuardTests")]
    public class GuardTests
    {
        [Theory]
        [InlineData("ValidName")]
        [InlineData("AnotherValidName")]
        [InlineData("Valid Name")]
        public void ContainsOnlyLettersAndSpaces_ValidCases_ReturnsTrue(string input)
        {
            // Act
            bool isValid = Guard.ContainsOnlyLettersAndSpaces(input);
            
            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ContainsOnlyLettersAndSpaces_NullInput_ReturnsFalse()
        {
            // Act
            bool isValid = Guard.ContainsOnlyLettersAndSpaces(null);
            
            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Invalid@")] 
        [InlineData(".invalid")]
        [InlineData("wow123invalid")]
        [InlineData("super_invalid")]
        [InlineData("invalid!")]
        [InlineData("inv alid?")]
        [InlineData("-")]
        public void ContainsOnlyLettersAndSpaces_InvalidCases_ReturnsFalse(string input)
        {
            // Act
            bool isValid = Guard.ContainsOnlyLettersAndSpaces(input);
            
            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("ValidPass123")]
        [InlineData("AnotherValid123")]
        [InlineData("123ValidPass")]
        public void ContainsOnlyAlphanumerics_ValidCases_ReturnsTrue(string input)
        {           
            // Act
            bool isValid = Guard.ContainsOnlyAlphanumerics(input);
            
            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ContainsOnlyAlphanumerics_NullInput_ReturnsFalse()
        {
            // Act
            bool isValid = Guard.ContainsOnlyAlphanumerics(null);
            
            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Invalid@")]
        [InlineData(".invalid")]
        [InlineData("super_invalid")]
        [InlineData("invalid!")]
        [InlineData("inv alid?")]
        [InlineData("-")]
        public void ContainsOnlyAlphanumerics_InvalidCases_ReturnsFalse(string input)
        {
            // Act
            bool isValid = Guard.ContainsOnlyAlphanumerics(input);
            
            // Assert
            Assert.False(isValid);
        }


    }
}
