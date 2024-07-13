using Domain.Entities.SignUpRequests;
using Domain.Shared;
using Unit.Tests.DataSources;

namespace Unit.Tests.Domain.SignUpRequestTests
{
    [Collection("SignUpRequestTests")]
    public class SignUpRequestTest
    {

        public SignUpRequestTest()
        {
            Environment.SetEnvironmentVariable("ALLOWED_DOMAINS", "isep.ipp.pt,gmail.com");
            Environment.SetEnvironmentVariable("MAX_NAME_LENGTH", "20");
        }

        [Theory]
        [InlineData("someIamId", "1211076@isep.ipp.pt", "John Doe", "247677949", "913756104")]
        [InlineData("someIamId", "1211076@isep.ipp.pt", "Mary Jane", "172658462", "923756104")]
        public void Constructor_ValidCases_ShouldCreateSignUpRequest(string iamId, string email, string name, string nif, string phoneNumber)
        {
            // Act
            SignUpRequest signUpRequest = new(iamId, email, name, nif, phoneNumber);

            // Assert
            Assert.NotNull(signUpRequest);
            Assert.Equal(iamId, signUpRequest.IamId);
            Assert.Equal(email, signUpRequest.Email.Value);
            Assert.Equal(name, signUpRequest.Name.Value);
            Assert.Equal(nif, signUpRequest.Nif.Value);
            Assert.Equal(phoneNumber, signUpRequest.PhoneNumber.Value);
            Assert.Equal(SignUpRequestStatus.Requested, signUpRequest.Status);
            Assert.True(signUpRequest.CreationTime <= DateTime.Now);
            Assert.True(signUpRequest.ActionTime <= DateTime.Now);
            Assert.Null(signUpRequest.ActionedBy);
            Assert.Null(signUpRequest.ActionComment);
        }

        [Theory]
        [InlineData("someIamId", "1211076isep.ipp.pt", "Mary Jane", "172658462", "923756104")]
        public void Constructor_InvalidEmail_ShouldThrowArgumentException(string iamId, string email, string name, string nif, string phoneNumber)
        {
            // Assert
            Assert.Throws<BusinessRuleValidationException>(() => new SignUpRequest(iamId, email, name, nif, phoneNumber));
        }


        [Theory]
        [InlineData("someIamId", "1211076@gmail.com", "John Doe", "2476779458589", "913756104")]
        [InlineData("someIamId", "1211076@gmail.com", "John Doe", "247678949", "913756104")]
        [InlineData("someIamId", "1211076@gmail.com", "John Doe", "invalid_nif", "913756104")]
        public void Constructor_InvalidNif_ShouldThrowArgumentException(string iamId, string email, string name, string nif, string phoneNumber)
        {
            // Assert
            Assert.Throws<BusinessRuleValidationException>(() => new SignUpRequest(iamId, email, name, nif, phoneNumber));
        }


        [Theory]
        [InlineData("someIamId", "1211076@gmail.com", "@John Doe", "247677949", "913756104")]
        [InlineData("someIamId", "1211076@gmail.com", "@____", "247677949", "913756104")]
        public void Constructor_InvalidName_ShouldThrowArgumentException(string iamId, string email, string name, string nif, string phoneNumber)
        {
            // Assert
            Assert.Throws<BusinessRuleValidationException>(() => new SignUpRequest(iamId, email, name, nif, phoneNumber));
        }


        [Theory]
        [InlineData("someIamId", "1211076@gmail.com", "John Doe", "247677949", "942782461")]
        [InlineData("someIamId", "1211076@gmail.com", "John Doe", "247677949", "9427824689191")]
        [InlineData("someIamId", "1211076@gmail.com", "John Doe", "247677949", "invalid_number")]
        public void Constructor_InvalidPhoneNumber_ShouldThrowArgumentException(string iamId, string email, string name, string nif, string phoneNumber)
        {
            // Assert
            Assert.Throws<BusinessRuleValidationException>(() => new SignUpRequest(iamId, email, name, nif, phoneNumber));
        }

        [Fact]
        public void Approve_SigUnRequest_ShouldChangeStatusToApproved()
        {
            // Arrange
            SignUpRequest signUpRequest = SignUpRequestDataSource.SignUpRequestA();

            // Act
            signUpRequest.Approve(UserDataSource.CampusManagerA(), "You are in my friend");

            // Assert
            Assert.Equal(SignUpRequestStatus.Approved, signUpRequest.Status);
        }


        [Fact]
        public void Reject_SigUnRequest_ShouldChangeStatusToRejected()
        {
            // Arrange
            SignUpRequest signUpRequest = SignUpRequestDataSource.SignUpRequestA();

            // Act
            signUpRequest.Reject(UserDataSource.CampusManagerA(), "You are not in my friend");

            // Assert
            Assert.Equal(SignUpRequestStatus.Rejected, signUpRequest.Status);
        }


    }
}
