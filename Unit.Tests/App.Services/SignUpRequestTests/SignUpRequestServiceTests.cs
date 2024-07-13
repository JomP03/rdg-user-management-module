using App.Services.Repositories;
using App.Services.Repositories.Shared;
using App.Services.Services.Users;
using Domain.Entities.Users;
using Unit.Tests.DataSources;
using IAM.Factories;
using IAM.Gateways.Users;
using Moq;
using App.Services.Mappers;
using App.Services.Dtos;
using IAM.Models;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.SignUpRequests;
using App.Services.Services.SignUpRequests;
using NSubstitute;


namespace Unit.Tests.App.Services.SignUpRequests
{
    [Collection("SignUpRequestServiceTests")]
    public class SignUpRequestServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<ISignUpRequestRepository> signUpRequestRepositoryMock;
        private readonly Mock<IUserService> myUserServiceMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<ISignUpRequestMapper> signUpRequestMapperMock;
        


        private readonly SignUpRequestService signUpRequestService;

        public SignUpRequestServiceTests()
        {
            // Initialize mocks
            unitOfWorkMock = new Mock<IUnitOfWork>();
            signUpRequestRepositoryMock = new Mock<ISignUpRequestRepository>();
            myUserServiceMock = new Mock<IUserService>();
            userRepositoryMock = new Mock<IUserRepository>();
            signUpRequestMapperMock = new Mock<ISignUpRequestMapper>();

            // Configure mocks
            unitOfWorkMock.SetupGet(u => u.SignUpRequests).Returns(signUpRequestRepositoryMock.Object);
            unitOfWorkMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);


            // Set the environment variables
            Environment.SetEnvironmentVariable("AUTH0_DOMAIN", "someDomain");
            Environment.SetEnvironmentVariable("AUTH0_TOKEN", "someToken");
            Environment.SetEnvironmentVariable("AUTH0_CONNECTION_DATABASE_NAME", "someConnectionDatabaseName");
            Environment.SetEnvironmentVariable("ALLOWED_DOMAINS", "isep.ipp.pt,gmail.com");
            Environment.SetEnvironmentVariable("MAX_NAME_LENGTH", "30");

            // Create the UserService instance with mocked dependencies
            signUpRequestService = new SignUpRequestService(unitOfWorkMock.Object, myUserServiceMock.Object);
        }

        [Fact]
        public void Dispose()
        {
            unitOfWorkMock.Reset();
            signUpRequestRepositoryMock.Reset();
        }

        [Fact]
        public async Task CreateSignUpRequestAsync_SuccessfulAddition_ReturnsSignUpRequestResponse()
        {

            var requestDto = SignUpRequestDataSource.CreateSignUpRequestRequestDtoA();

            // Act
            var response = await signUpRequestService.CreateSignUpRequest(requestDto);

            // Assert
            Assert.Equal(SignUpRequestDataSource.CreateSignUpRequestRequestDtoA().Nif, response.Nif);
            Assert.Equal(SignUpRequestDataSource.CreateSignUpRequestRequestDtoA().PhoneNumber, response.PhoneNumber);
            Assert.Equal(SignUpRequestDataSource.CreateSignUpRequestRequestDtoA().Email, response.Email);
        }


        [Fact]
        public async Task AddUserAsync_DbUpdateExceptionThrownWithInnerExceptionPhoneNumber_ThrowsEntityAlreadyExistsException()
        {
            //Arrange

            userRepositoryMock.Setup(ur => ur.CheckForExistingUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).ThrowsAsync(new DbUpdateException("someMessage", new Exception("IX_SignUpRequests_PhoneNumber")));

            var requestDto = SignUpRequestDataSource.CreateSignUpRequestRequestDtoA();

            // Act
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => signUpRequestService.CreateSignUpRequest(requestDto));
        }

        [Fact]
        public async Task AddUserAsync_DbUpdateExceptionThrownWithInnerExceptionNif_ThrowsEntityAlreadyExistsException()
        {
            //Arrange

            userRepositoryMock.Setup(ur => ur.CheckForExistingUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                )).ThrowsAsync(new DbUpdateException("someMessage", new Exception("IX_SignUpRequests_Nif")));


            var requestDto = SignUpRequestDataSource.CreateSignUpRequestRequestDtoA();

            // Act
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => signUpRequestService.CreateSignUpRequest(requestDto));
        }

        [Fact]
        public async Task AddUserAsync_DbUpdateExceptionThrownWithInnerExceptionEmail_ThrowsEntityAlreadyExistsException()
        {
            // Arrange

            userRepositoryMock.Setup(ur => ur.CheckForExistingUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                )).ThrowsAsync(new DbUpdateException("someMessage", new Exception("IX_SignUpRequests_Email")));


            var requestDto = SignUpRequestDataSource.CreateSignUpRequestRequestDtoA();

            // Act
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => signUpRequestService.CreateSignUpRequest(requestDto));
        }


        [Fact]
        public async Task AcceptOrRejectSignUpRequest_ApproveSuccessfully()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            // MOCK SIGNUPREQUEST REPO
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);
       
            // MOCK USER REPO
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());

            // MOCK USER SERVICE
            myUserServiceMock.Setup(ur => ur.AddEndUserAsync(It.IsAny<SignUpRequest>())).ReturnsAsync(UserDataSource.UserResponseA());

            // MOCK SIGNUPREQUEST MAPPER
            signUpRequestMapperMock.Setup(ur => ur.MapFromDomainToOutDto(It.IsAny<SignUpRequest>())).Returns(SignUpRequestDataSource.CreateSignUpRequestResponseDtoA());

            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestApprove();

            // Act
            var response = await signUpRequestService.AcceptOrRejectSignUpRequest(id,dto);

            // Assert
            Assert.Equal(SignUpRequestStatus.Approved.ToString(), response.Status.ToString());
        }

        [Fact]
        public async Task AcceptOrRejectSignUpRequest_RejectSuccessfully()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            // MOCK SIGNUPREQUEST REPO
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);

            // MOCK USER REPO
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());

            // MOCK USER SERVICE
            myUserServiceMock.Setup(ur => ur.AddEndUserAsync(It.IsAny<SignUpRequest>())).ReturnsAsync(UserDataSource.UserResponseA());

            // MOCK SIGNUPREQUEST MAPPER
            signUpRequestMapperMock.Setup(ur => ur.MapFromDomainToOutDto(It.IsAny<SignUpRequest>())).Returns(SignUpRequestDataSource.CreateSignUpRequestResponseDtoA());

            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestReject();

            // Act
            var response = await signUpRequestService.AcceptOrRejectSignUpRequest(id, dto);

            // Assert
            Assert.Equal(SignUpRequestStatus.Rejected.ToString(), response.Status.ToString());
        }


        [Fact]
        public async Task AcceptOrRejectSignUpRequest_SignUpRequestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            // MOCK SIGNUPREQUEST REPO
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ThrowsAsync(new EntityNotFoundException("SignUpRequest", "id", "someId"));


            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestReject();

            // Act / Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => signUpRequestService.AcceptOrRejectSignUpRequest(id, dto));

        }

        [Fact]
        public async Task AcceptOrRejectSignUpRequest_UserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            // MOCK SIGNUPREQUEST REPO
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);

            // MOCK USER REPO
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ThrowsAsync(new EntityNotFoundException("User", "iamId", "someIamId"));

            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestReject();

            // Act / Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => signUpRequestService.AcceptOrRejectSignUpRequest(id, dto));

        }





    }
}
