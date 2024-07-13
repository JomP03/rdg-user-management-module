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
using Domain.Shared;
using Microsoft.VisualStudio.TestPlatform.Common;


namespace Unit.Tests.App.Services.UserTests
{
    [Collection("UserServiceTests")]
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IRoleRepository> roleRepositoryMock;
        private readonly Mock<IUserGatewayFactory> userGatewayFactoryMock;
        private readonly Mock<IUsersGateway> userGatewayMock;
        private readonly Mock<IUserMapper> userMapperMock;


        private readonly UserService userService;

        public UserServiceTests()
        {
            // Initialize mocks
            unitOfWorkMock = new Mock<IUnitOfWork>();
            userRepositoryMock = new Mock<IUserRepository>();
            roleRepositoryMock = new Mock<IRoleRepository>();
            userGatewayFactoryMock = new Mock<IUserGatewayFactory>();
            userGatewayMock = new Mock<IUsersGateway>();
            userMapperMock = new Mock<IUserMapper>();

            // Configure mocks
            unitOfWorkMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            unitOfWorkMock.SetupGet(u => u.Roles).Returns(roleRepositoryMock.Object);
            userGatewayFactoryMock
                    .Setup(ugf => ugf.Create(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(() => userGatewayMock.Object);

            // Set the environment variables
            Environment.SetEnvironmentVariable("AUTH0_DOMAIN", "someDomain");
            Environment.SetEnvironmentVariable("AUTH0_TOKEN", "someToken");
            Environment.SetEnvironmentVariable("AUTH0_CONNECTION_DATABASE_NAME", "someConnectionDatabaseName");
            Environment.SetEnvironmentVariable("ALLOWED_DOMAINS", "isep.ipp.pt,gmail.com");
            Environment.SetEnvironmentVariable("MAX_NAME_LENGTH", "30");

            // Create the UserService instance with mocked dependencies
            userService = new UserService(unitOfWorkMock.Object, userGatewayFactoryMock.Object, userMapperMock.Object);
        }

        [Fact]
        public void Dispose()
        {
            unitOfWorkMock.Reset();
            userRepositoryMock.Reset();
            roleRepositoryMock.Reset();
            userGatewayFactoryMock.Reset();
            userGatewayMock.Reset();
            userMapperMock.Reset();
        }

        [Fact]
        public async Task AddUserAsync_SuccessfulAddition_ReturnsUserResponse()
        {
            // Arrange
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
            userMapperMock.Setup(um => um.MapFromCreateDtoToDomain(It.IsAny<CreateManagerUserRequestDto>())).Returns(UserDataSource.CampusManagerA());
            userMapperMock.Setup(um => um.MapFromDomainToOutDto(It.IsAny<User>())).Returns(UserDataSource.CampusManagerAResponse());
            userGatewayMock.Setup(ug => ug.CreateUserAsync(It.IsAny<Auth0User>())).ReturnsAsync(UserDataSource.Auth0UserResponse("someIamId"));
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var requestDto = UserDataSource.CampusManagerARequestDto();

            // Act
            var response = await userService.AddUserAsync(requestDto);

            // Assert
            Assert.Equal(UserDataSource.CampusManagerA().Name.Value, response.Name);
            Assert.Equal(UserDataSource.CampusManagerA().Email.Value, response.Email);
            Assert.Equal(UserDataSource.CampusManagerA().PhoneNumber.Value, response.PhoneNumber);
        }

        [Fact]
        public async Task AddUserAsync_RoleNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var requestDto = UserDataSource.CampusManagerARequestDto();
            // Note: the role repository mock is not configured to return a role so it will return null, which is the same
            // as mocking it to return null on every call

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => userService.AddUserAsync(requestDto));
        }

        [Fact]
        public async Task AddUserAsync_DbUpdateExceptionThrown_UserGatewayDeleteUserAsyncCalled()
        {
            // Arrange
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
            userMapperMock.Setup(um => um.MapFromCreateDtoToDomain(It.IsAny<CreateManagerUserRequestDto>())).Returns(UserDataSource.CampusManagerA());
            userMapperMock.Setup(um => um.MapFromDomainToOutDto(It.IsAny<User>())).Returns(UserDataSource.CampusManagerAResponse());
            userGatewayMock.Setup(ug => ug.CreateUserAsync(It.IsAny<Auth0User>())).ReturnsAsync(UserDataSource.Auth0UserResponse("someIamId"));
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).ThrowsAsync(new DbUpdateException());

            var requestDto = UserDataSource.CampusManagerARequestDto();

            // Act
            var exception = await Assert.ThrowsAsync<DbUpdateException>(() => userService.AddUserAsync(requestDto));

            // Assert
            userGatewayMock.Verify(ug => ug.DeleteUserAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_DbUpdateExceptionThrownWithInnerExceptionPhoneNumber_ThrowsEntityAlreadyExistsException()
        {
            // Arrange
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
            userMapperMock.Setup(um => um.MapFromCreateDtoToDomain(It.IsAny<CreateManagerUserRequestDto>())).Returns(UserDataSource.CampusManagerA());
            userMapperMock.Setup(um => um.MapFromDomainToOutDto(It.IsAny<User>())).Returns(UserDataSource.CampusManagerAResponse());
            userGatewayMock.Setup(ug => ug.CreateUserAsync(It.IsAny<Auth0User>())).ReturnsAsync(UserDataSource.Auth0UserResponse("someIamId"));
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            // Make add user throw a DbUpdateException with an inner exception with a message containing IX_Users_PhoneNumber
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).ThrowsAsync(new DbUpdateException("someMessage", new Exception("IX_Users_PhoneNumber")));


            var requestDto = UserDataSource.CampusManagerARequestDto();

            // Act
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => userService.AddUserAsync(requestDto));
        }

        [Fact]
        public async Task AddUserAsync_DbUpdateExceptionThrownWithInnerExceptionEmail_ThrowsEntityAlreadyExistsException()
        {
            // Arrange
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
            userMapperMock.Setup(um => um.MapFromCreateDtoToDomain(It.IsAny<CreateManagerUserRequestDto>())).Returns(UserDataSource.CampusManagerA());
            userMapperMock.Setup(um => um.MapFromDomainToOutDto(It.IsAny<User>())).Returns(UserDataSource.CampusManagerAResponse());
            userGatewayMock.Setup(ug => ug.CreateUserAsync(It.IsAny<Auth0User>())).ReturnsAsync(UserDataSource.Auth0UserResponse("someIamId"));
            userGatewayMock.Setup(ug => ug.AddRoleToUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>())).ThrowsAsync(new DbUpdateException("someMessage", new Exception("IX_Users_Email")));

            var requestDto = UserDataSource.CampusManagerARequestDto();

            // Act
            var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => userService.AddUserAsync(requestDto));
        }

        [Fact]
        public async Task UpdateUserAsync_SuccessfulUpdate_ReturnsUserResponse()
        {
            // Arrange
            var userId = "someEmail@gmail.com";
            var updateUserDto = new UpdateUserRequestDto
            {
                Name = "someName",
                PhoneNumber = "961672312",
                Nif = "535813244"
            };

            userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.EndUserA());
            userMapperMock.Setup(um => um.MapFromDomainToOutDto(It.IsAny<User>())).Returns(UserDataSource.CampusManagerAResponse);

            // Act
            var response = await userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.Equal(UserDataSource.CampusManagerA().Name.Value, response.Name);
            Assert.Equal(UserDataSource.CampusManagerA().PhoneNumber.Value, response.PhoneNumber);
        }

        [Fact]
        public async Task UpdateUserAsync_UserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var userId = "someEmail@gmail.com";
            var updateUserDto = new UpdateUserRequestDto
            {
                Name = "someName",
                PhoneNumber = "961672312",
                Nif = "535813244"
            };

            userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act/Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => userService.UpdateUserAsync(userId, updateUserDto));
        }

        [Fact]
        public async Task UpdateUserAsync_UserIsNotAnEndUserToUpdateNif_ThrowsBusinessRuleValidationException()
        {
            // Arrange
            var userId = "someEmail@gmail.com";
            var updateUserDto = new UpdateUserRequestDto
            {
                Name = "someName",
                PhoneNumber = "961672312",
                Nif = "535813244"
            };

            userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserDataSource.CampusManagerA());
            userMapperMock.Setup(um => um.MapFromDomainToOutDto(It.IsAny<User>())).Returns(UserDataSource.CampusManagerAResponse);


            // Act/Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => userService.UpdateUserAsync(userId, updateUserDto));

        }

        [Theory]
        [InlineData("someEmail@isep.ipp.pt", "John Doe", "9123456", "535813244")]
        [InlineData("someEmail@isep.ipp.pt", "John Doe", "912345678", "000000001")]
        [InlineData("someEmail@isep.ipp.pt", "John Doe !", "912345678", "535813244")]
        public async Task UpdateUserAsync_InvalidUpdateUserRequestDto_ThrowsBusinessRuleValidationException(string email, string name, string phoneNumber, string nif)
        {
            // Arrange
            var user = UserDataSource.EndUserA();
            userRepositoryMock.Setup(u => u.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            userMapperMock.Setup(um => um.MapFromDomainToOutDto(It.IsAny<User>())).Returns(UserDataSource.CampusManagerAResponse);

            var updateUserDto = new UpdateUserRequestDto
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Nif = nif
            };

            // Act, Assert
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => userService.UpdateUserAsync(email, updateUserDto));
        }




    }
}
