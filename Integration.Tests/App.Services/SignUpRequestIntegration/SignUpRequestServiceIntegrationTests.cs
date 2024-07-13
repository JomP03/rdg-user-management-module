using App.Services.Dtos;
using App.Services.Mappers;
using App.Services.Repositories;
using App.Services.Repositories.Shared;
using App.Services.Services.SignUpRequests;
using App.Services.Services.Users;
using Domain.Entities.SignUpRequests;
using Domain.Entities.Users;
using Domain.Shared;
using IAM.Factories;
using IAM.Gateways.Users;
using IAM.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Unit.Tests.DataSources;

namespace Integration.Tests.App.Services.SignUpRequestIntegration
{
    [Collection("Repository Mock")]
    public class SignUpRequestServiceIntegrationTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<ISignUpRequestRepository> signUpRequestRepositoryMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<ISignUpRequestMapper> signUpRequestMapperMock;
        private readonly Mock<IUserMapper> userMapperMock;
        private readonly Mock<IRoleRepository> roleRepositoryMock;
        private readonly Mock<IUserGatewayFactory> userGatewayFactoryMock;
        private readonly Mock<IUsersGateway> userGatewayMock;



        private readonly SignUpRequestService signUpRequestService;

        public SignUpRequestServiceIntegrationTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            signUpRequestRepositoryMock = new Mock<ISignUpRequestRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            signUpRequestMapperMock = new Mock<ISignUpRequestMapper>();
            roleRepositoryMock = new Mock<IRoleRepository>();
            userGatewayFactoryMock = new Mock<IUserGatewayFactory>();
            userGatewayMock = new Mock<IUsersGateway>();
            userMapperMock = new Mock<IUserMapper>();

            unitOfWorkMock.Setup(u => u.SignUpRequests).Returns(signUpRequestRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.Roles).Returns(roleRepositoryMock.Object);
            userGatewayFactoryMock
                    .Setup(ugf => ugf.Create(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(() => userGatewayMock.Object);

            // Set the environment variables
            Environment.SetEnvironmentVariable("AUTH0_DOMAIN", "someDomain");
            Environment.SetEnvironmentVariable("AUTH0_TOKEN", "someToken");
            Environment.SetEnvironmentVariable("AUTH0_CONNECTION_DATABASE_NAME", "someConnectionDatabaseName");
            Environment.SetEnvironmentVariable("ALLOWED_DOMAINS", "isep.ipp.pt,gmail.com");
            Environment.SetEnvironmentVariable("MAX_NAME_LENGTH", "30");

            signUpRequestService = new SignUpRequestService(unitOfWorkMock.Object, new UserService(unitOfWorkMock.Object,userGatewayFactoryMock.Object, userMapperMock.Object));
        }

        [Fact]
        public void Dispose()
        {
            unitOfWorkMock.Reset();
            signUpRequestRepositoryMock.Reset();
        }

        [Fact]
        public async Task CreateSignUpRequestAsync_WhenCalled_ReturnsSignUpRequest()
        {
            // Arrange
            var requestDTO = SignUpRequestDataSource.CreateSignUpRequestRequestDtoA();

            // Act
            var result = await signUpRequestService.CreateSignUpRequest(requestDTO);

            // Assert
            Assert.Equal(SignUpRequestDataSource.SignUpRequestA().IamId, result.IamId);
            Assert.Equal(SignUpRequestDataSource.SignUpRequestA().Email.Value, result.Email);
            Assert.Equal(SignUpRequestDataSource.SignUpRequestA().Name.Value, result.Name);
            Assert.Equal(SignUpRequestDataSource.SignUpRequestA().Nif.Value, result.Nif);
            Assert.Equal(SignUpRequestDataSource.SignUpRequestA().PhoneNumber.Value, result.PhoneNumber);
        }


        [Theory]
        [InlineData("iamId1", "teste@isep.ipp.pt", "John Doe", "13756104", "932782411")]
        [InlineData("iamId2", "testegmail.co", "Mary Jean", "923756104", "932782412")]
        [InlineData("iamId3", "teste@isep.ipp.pt", "Mary @@Jean", "923756104", "932782413")]
        [InlineData("iamId4", "teste@isep.ipp.pt", "John Doe", "2582582582", "932782414")]
        [InlineData("iamId4", "teste@isep.ipp.pt", "John Doe", "247677949", "993369369")]
        public async Task AddUserAsync_InvalidUserReceivedAsParameter_ThrowsBusinessRuleValidationException(string iamId, string email, string name, string nif, string phoneNumber)
        {
            // Arrange
            var requestDTO = new CreateSignUpRequestRequestDto
            {
                IamId = iamId,
                Email = email,
                Name = name,
                Nif = nif,
                PhoneNumber = phoneNumber
            };

            // Assert
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => signUpRequestService.CreateSignUpRequest(requestDTO));
        }


        [Fact]
        public async Task AcceptOrRejectSignUpRequest_RejectSuccessfully()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            //MOCK REPOS
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());
            roleRepositoryMock.Setup(r => r.GetEndUserRoleAsync()).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

         
            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestReject();

            // Act
            var response = await signUpRequestService.AcceptOrRejectSignUpRequest(id, dto);

            // Assert
            Assert.Equal(SignUpRequestStatus.Rejected.ToString(), response.Status.ToString());
        }

        [Fact]
        public async Task AcceptOrRejectSignUpRequest_AcceptSuccessfully()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            //MOCK REPOS
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());
            roleRepositoryMock.Setup(r => r.GetEndUserRoleAsync()).ReturnsAsync(UserRoleDataSource.CampusManagerRole()); 
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestApprove();

            // Act
            var response = await signUpRequestService.AcceptOrRejectSignUpRequest(id, dto);

            // Assert
            Assert.Equal(SignUpRequestStatus.Approved.ToString(), response.Status.ToString());
        }

        [Fact]
        public async Task AcceptOrRejectSignUpRequest_NonUniquePhoneNumber_ThrowsDbException()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            //MOCK REPOS
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());
            roleRepositoryMock.Setup(r => r.GetEndUserRoleAsync()).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).Throws(new DbUpdateException("someMessage", new Exception("IX_Users_PhoneNumber")));
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestApprove();

            // Act / Assert
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => signUpRequestService.AcceptOrRejectSignUpRequest(id, dto));
        }


        [Fact]
        public async Task AcceptOrRejectSignUpRequest_NonUniqueEmail_ThrowsDbException()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            //MOCK REPOS
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());
            roleRepositoryMock.Setup(r => r.GetEndUserRoleAsync()).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).Throws(new DbUpdateException("someMessage", new Exception("IX_Users_Email")));
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestApprove();

            // Act / Assert
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => signUpRequestService.AcceptOrRejectSignUpRequest(id, dto));

        }


        [Fact]
        public async Task AcceptOrRejectSignUpRequest_NonUniqueNif_ThrowsDbException()
        {
            // Arrange
            var signupRequest = SignUpRequestDataSource.SignUpRequestA();

            //MOCK REPOS
            signUpRequestRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(signupRequest);
            userRepositoryMock.Setup(ur => ur.GetByIamIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());
            roleRepositoryMock.Setup(r => r.GetEndUserRoleAsync()).ReturnsAsync(UserRoleDataSource.EndUserRole());
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).Throws(new DbUpdateException("someMessage", new Exception("IX_Users_Nif")));
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            string id = "someId";

            SignUpRequestActionDto dto = SignUpRequestDataSource.CreateSignUpRequestApprove();

            // Act / Assert
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => signUpRequestService.AcceptOrRejectSignUpRequest(id, dto));

        }

    }
}
