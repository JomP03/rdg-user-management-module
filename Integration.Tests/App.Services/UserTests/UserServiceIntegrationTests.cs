using App.Services.Repositories;
using App.Services.Repositories.Shared;
using App.Services.Services.Users;
using Unit.Tests.DataSources;
using IAM.Factories;
using IAM.Gateways.Users;
using Moq;
using App.Services.Mappers;
using App.Services.Dtos;
using IAM.Models;
using Domain.Shared;
using Integration.Tests.Helpers;
using Domain.Entities.Users.Role;
using IAM.Exceptions;
using Domain.Entities.Users;


namespace Integration.Tests.App.Services.UserTests
{
    [Collection("UserServiceIntegrationTestsWithMockedGateway")]
    public class UserServiceIntegrationTestsMockedGateway
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IRoleRepository> roleRepositoryMock;
        private readonly Mock<IUserGatewayFactory> userGatewayFactoryMock;
        private readonly Mock<IUsersGateway> userGatewayMock;


        private readonly UserService userService;

        public UserServiceIntegrationTestsMockedGateway()
        {
            // Initialize mocks
            unitOfWorkMock = new Mock<IUnitOfWork>();
            userRepositoryMock = new Mock<IUserRepository>();
            roleRepositoryMock = new Mock<IRoleRepository>();
            userGatewayFactoryMock = new Mock<IUserGatewayFactory>();
            userGatewayMock = new Mock<IUsersGateway>();
            var userMapper = new UserMapper();

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
            userService = new UserService(unitOfWorkMock.Object, userGatewayFactoryMock.Object, userMapper);
        }

        [Fact]
        public void Dispose()
        {
            unitOfWorkMock.Reset();
            userRepositoryMock.Reset();
            roleRepositoryMock.Reset();
            userGatewayFactoryMock.Reset();
            userGatewayMock.Reset();
        }

        [Fact]
        public async Task AddUserAsync_SuccessfulAddition_ReturnsUserResponse()
        {
            // Arrange
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserRoleDataSource.CampusManagerRole());
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

        // Tests similar to the one above but with invalid users received as parameters
        [Theory]
        [InlineData("teste@isep.ipp.pt", "John Doe", "13756104", "ValidPass123!")]
        [InlineData("teste@gmail.co", "Mary Jean", "923756104", "SuperSafe1!89")]
        [InlineData("teste@gmail.com", "Mary Jean", "923756104", "SuperSafe189")]
        [InlineData("teste@isep.ipp.pt", "John 1Doe", "913756104", "ValidPass123!")]
        public async Task AddUserAsync_InvalidUserReceivedAsParameter_ThrowsBusinessRuleValidationException(string email, string name, string phoneNumber, string password)
        {
            // Arrange
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UserRoleDataSource.CampusManagerRole());

            var requestDto = new CreateManagerUserRequestDto
            {
                Email = email,
                Name = name,
                PhoneNumber = phoneNumber,
                Password = password,
                RoleId = UserRoleDataSource.CampusManagerRole().Id.ToString()
            };

            // Act, Assert
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => userService.AddUserAsync(requestDto));
        }

        [Fact]
        public async Task UpdateUserAsync_SuccessfulUpdate_ReturnsUserResponse()
        {
            // Arrange
            var user = UserDataSource.EndUserA();
            userRepositoryMock.Setup(u => u.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

     
            var updateUserDto = new UpdateUserRequestDto
            {
                Name = "someName",
                PhoneNumber = "961672312",
                Nif = "535813244"
            };


            // Act
            var response = await userService.UpdateUserAsync(user.Email.Value, updateUserDto);

            // Assert
            Assert.Equal(updateUserDto.Name, response.Name);
            Assert.Equal(user.Email.Value, response.Email);
            Assert.Equal(updateUserDto.PhoneNumber, response.PhoneNumber);
        }

        [Fact]
        public async Task UpdateUserAsync_UserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            userRepositoryMock.Setup(u => u.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            var updateUserDto = new UpdateUserRequestDto
            {
                Name = "someName",
                PhoneNumber = "961672312",
                Nif = "535813244"
            };

            // Act, Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => userService.UpdateUserAsync("someEmail", updateUserDto));
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateNifInNonEndUser_ThrowsBusinessRuleValidationException()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();
            userRepositoryMock.Setup(u => u.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            var updateUserDto = new UpdateUserRequestDto
            {
                Name = "someName",
                PhoneNumber = "961672312",
                Nif = "535813244"
            };

            // Act, Assert
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => userService.UpdateUserAsync(user.Email.Value, updateUserDto));
        }

        [Theory]
        [InlineData("someEmail@isep.ipp.pt","John Doe","9123456", "535813244")]
        [InlineData("someEmail@isep.ipp.pt", "John Doe", "912345678", "000000001")]
        [InlineData("someEmail@isep.ipp.pt", "John Doe !", "912345678", "535813244")]
        public async Task UpdateUserAsync_InvalidUserReceivedAsParameter_ThrowsBusinessRuleValidationException(string email, string name, string phoneNumber, string nif)
        {
            // Arrange
            var user = UserDataSource.EndUserA();
            userRepositoryMock.Setup(u => u.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

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


    public class UserServiceIntegrationTestsRealGateway
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IRoleRepository> roleRepositoryMock;

        private readonly UserService userService;

        public UserServiceIntegrationTestsRealGateway()
        {
            // Initialize mocks
            unitOfWorkMock = new Mock<IUnitOfWork>();
            userRepositoryMock = new Mock<IUserRepository>();
            roleRepositoryMock = new Mock<IRoleRepository>();
            var userMapper = new UserMapper();
            var userGatewayFactory = new UserGatewayFactory(new HttpClientFactory());

            // Configure mocks
            unitOfWorkMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            unitOfWorkMock.SetupGet(u => u.Roles).Returns(roleRepositoryMock.Object);

            // Set the environment variables
            Environment.SetEnvironmentVariable("AUTH0_DOMAIN", "https://dev-51jlmax5m1sebppq.eu.auth0.com");
            Environment.SetEnvironmentVariable("AUTH0_TOKEN", "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImFKOGJkVF81N2JuR1pTZEI0djRPSCJ9.eyJpc3MiOiJodHRwczovL2Rldi01MWpsbWF4NW0xc2VicHBxLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJpSlhvRUZqZnA2eHhsRDVmNmJTNU55MTdIYXBYOUFSbkBjbGllbnRzIiwiYXVkIjoiaHR0cHM6Ly9kZXYtNTFqbG1heDVtMXNlYnBwcS5ldS5hdXRoMC5jb20vYXBpL3YyLyIsImlhdCI6MTcwMjY4ODY2MSwiZXhwIjoxNzAzOTg0NjYxLCJhenAiOiJpSlhvRUZqZnA2eHhsRDVmNmJTNU55MTdIYXBYOUFSbiIsInNjb3BlIjoicmVhZDpjbGllbnRfZ3JhbnRzIGNyZWF0ZTpjbGllbnRfZ3JhbnRzIGRlbGV0ZTpjbGllbnRfZ3JhbnRzIHVwZGF0ZTpjbGllbnRfZ3JhbnRzIHJlYWQ6dXNlcnMgdXBkYXRlOnVzZXJzIGRlbGV0ZTp1c2VycyBjcmVhdGU6dXNlcnMgcmVhZDp1c2Vyc19hcHBfbWV0YWRhdGEgdXBkYXRlOnVzZXJzX2FwcF9tZXRhZGF0YSBkZWxldGU6dXNlcnNfYXBwX21ldGFkYXRhIGNyZWF0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgcmVhZDp1c2VyX2N1c3RvbV9ibG9ja3MgY3JlYXRlOnVzZXJfY3VzdG9tX2Jsb2NrcyBkZWxldGU6dXNlcl9jdXN0b21fYmxvY2tzIGNyZWF0ZTp1c2VyX3RpY2tldHMgcmVhZDpjbGllbnRzIHVwZGF0ZTpjbGllbnRzIGRlbGV0ZTpjbGllbnRzIGNyZWF0ZTpjbGllbnRzIHJlYWQ6Y2xpZW50X2tleXMgdXBkYXRlOmNsaWVudF9rZXlzIGRlbGV0ZTpjbGllbnRfa2V5cyBjcmVhdGU6Y2xpZW50X2tleXMgcmVhZDpjb25uZWN0aW9ucyB1cGRhdGU6Y29ubmVjdGlvbnMgZGVsZXRlOmNvbm5lY3Rpb25zIGNyZWF0ZTpjb25uZWN0aW9ucyByZWFkOnJlc291cmNlX3NlcnZlcnMgdXBkYXRlOnJlc291cmNlX3NlcnZlcnMgZGVsZXRlOnJlc291cmNlX3NlcnZlcnMgY3JlYXRlOnJlc291cmNlX3NlcnZlcnMgcmVhZDpkZXZpY2VfY3JlZGVudGlhbHMgdXBkYXRlOmRldmljZV9jcmVkZW50aWFscyBkZWxldGU6ZGV2aWNlX2NyZWRlbnRpYWxzIGNyZWF0ZTpkZXZpY2VfY3JlZGVudGlhbHMgcmVhZDpydWxlcyB1cGRhdGU6cnVsZXMgZGVsZXRlOnJ1bGVzIGNyZWF0ZTpydWxlcyByZWFkOnJ1bGVzX2NvbmZpZ3MgdXBkYXRlOnJ1bGVzX2NvbmZpZ3MgZGVsZXRlOnJ1bGVzX2NvbmZpZ3MgcmVhZDpob29rcyB1cGRhdGU6aG9va3MgZGVsZXRlOmhvb2tzIGNyZWF0ZTpob29rcyByZWFkOmFjdGlvbnMgdXBkYXRlOmFjdGlvbnMgZGVsZXRlOmFjdGlvbnMgY3JlYXRlOmFjdGlvbnMgcmVhZDplbWFpbF9wcm92aWRlciB1cGRhdGU6ZW1haWxfcHJvdmlkZXIgZGVsZXRlOmVtYWlsX3Byb3ZpZGVyIGNyZWF0ZTplbWFpbF9wcm92aWRlciBibGFja2xpc3Q6dG9rZW5zIHJlYWQ6c3RhdHMgcmVhZDppbnNpZ2h0cyByZWFkOnRlbmFudF9zZXR0aW5ncyB1cGRhdGU6dGVuYW50X3NldHRpbmdzIHJlYWQ6bG9ncyByZWFkOmxvZ3NfdXNlcnMgcmVhZDpzaGllbGRzIGNyZWF0ZTpzaGllbGRzIHVwZGF0ZTpzaGllbGRzIGRlbGV0ZTpzaGllbGRzIHJlYWQ6YW5vbWFseV9ibG9ja3MgZGVsZXRlOmFub21hbHlfYmxvY2tzIHVwZGF0ZTp0cmlnZ2VycyByZWFkOnRyaWdnZXJzIHJlYWQ6Z3JhbnRzIGRlbGV0ZTpncmFudHMgcmVhZDpndWFyZGlhbl9mYWN0b3JzIHVwZGF0ZTpndWFyZGlhbl9mYWN0b3JzIHJlYWQ6Z3VhcmRpYW5fZW5yb2xsbWVudHMgZGVsZXRlOmd1YXJkaWFuX2Vucm9sbG1lbnRzIGNyZWF0ZTpndWFyZGlhbl9lbnJvbGxtZW50X3RpY2tldHMgcmVhZDp1c2VyX2lkcF90b2tlbnMgY3JlYXRlOnBhc3N3b3Jkc19jaGVja2luZ19qb2IgZGVsZXRlOnBhc3N3b3Jkc19jaGVja2luZ19qb2IgcmVhZDpjdXN0b21fZG9tYWlucyBkZWxldGU6Y3VzdG9tX2RvbWFpbnMgY3JlYXRlOmN1c3RvbV9kb21haW5zIHVwZGF0ZTpjdXN0b21fZG9tYWlucyByZWFkOmVtYWlsX3RlbXBsYXRlcyBjcmVhdGU6ZW1haWxfdGVtcGxhdGVzIHVwZGF0ZTplbWFpbF90ZW1wbGF0ZXMgcmVhZDptZmFfcG9saWNpZXMgdXBkYXRlOm1mYV9wb2xpY2llcyByZWFkOnJvbGVzIGNyZWF0ZTpyb2xlcyBkZWxldGU6cm9sZXMgdXBkYXRlOnJvbGVzIHJlYWQ6cHJvbXB0cyB1cGRhdGU6cHJvbXB0cyByZWFkOmJyYW5kaW5nIHVwZGF0ZTpicmFuZGluZyBkZWxldGU6YnJhbmRpbmcgcmVhZDpsb2dfc3RyZWFtcyBjcmVhdGU6bG9nX3N0cmVhbXMgZGVsZXRlOmxvZ19zdHJlYW1zIHVwZGF0ZTpsb2dfc3RyZWFtcyBjcmVhdGU6c2lnbmluZ19rZXlzIHJlYWQ6c2lnbmluZ19rZXlzIHVwZGF0ZTpzaWduaW5nX2tleXMgcmVhZDpsaW1pdHMgdXBkYXRlOmxpbWl0cyBjcmVhdGU6cm9sZV9tZW1iZXJzIHJlYWQ6cm9sZV9tZW1iZXJzIGRlbGV0ZTpyb2xlX21lbWJlcnMgcmVhZDplbnRpdGxlbWVudHMgcmVhZDphdHRhY2tfcHJvdGVjdGlvbiB1cGRhdGU6YXR0YWNrX3Byb3RlY3Rpb24gcmVhZDpvcmdhbml6YXRpb25zX3N1bW1hcnkgY3JlYXRlOmF1dGhlbnRpY2F0aW9uX21ldGhvZHMgcmVhZDphdXRoZW50aWNhdGlvbl9tZXRob2RzIHVwZGF0ZTphdXRoZW50aWNhdGlvbl9tZXRob2RzIGRlbGV0ZTphdXRoZW50aWNhdGlvbl9tZXRob2RzIHJlYWQ6b3JnYW5pemF0aW9ucyB1cGRhdGU6b3JnYW5pemF0aW9ucyBjcmVhdGU6b3JnYW5pemF0aW9ucyBkZWxldGU6b3JnYW5pemF0aW9ucyBjcmVhdGU6b3JnYW5pemF0aW9uX21lbWJlcnMgcmVhZDpvcmdhbml6YXRpb25fbWVtYmVycyBkZWxldGU6b3JnYW5pemF0aW9uX21lbWJlcnMgY3JlYXRlOm9yZ2FuaXphdGlvbl9jb25uZWN0aW9ucyByZWFkOm9yZ2FuaXphdGlvbl9jb25uZWN0aW9ucyB1cGRhdGU6b3JnYW5pemF0aW9uX2Nvbm5lY3Rpb25zIGRlbGV0ZTpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgY3JlYXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJfcm9sZXMgcmVhZDpvcmdhbml6YXRpb25fbWVtYmVyX3JvbGVzIGRlbGV0ZTpvcmdhbml6YXRpb25fbWVtYmVyX3JvbGVzIGNyZWF0ZTpvcmdhbml6YXRpb25faW52aXRhdGlvbnMgcmVhZDpvcmdhbml6YXRpb25faW52aXRhdGlvbnMgZGVsZXRlOm9yZ2FuaXphdGlvbl9pbnZpdGF0aW9ucyBkZWxldGU6cGhvbmVfcHJvdmlkZXJzIGNyZWF0ZTpwaG9uZV9wcm92aWRlcnMgcmVhZDpwaG9uZV9wcm92aWRlcnMgdXBkYXRlOnBob25lX3Byb3ZpZGVycyBkZWxldGU6cGhvbmVfdGVtcGxhdGVzIGNyZWF0ZTpwaG9uZV90ZW1wbGF0ZXMgcmVhZDpwaG9uZV90ZW1wbGF0ZXMgdXBkYXRlOnBob25lX3RlbXBsYXRlcyBjcmVhdGU6ZW5jcnlwdGlvbl9rZXlzIHJlYWQ6ZW5jcnlwdGlvbl9rZXlzIHVwZGF0ZTplbmNyeXB0aW9uX2tleXMgZGVsZXRlOmVuY3J5cHRpb25fa2V5cyByZWFkOnNlc3Npb25zIGRlbGV0ZTpzZXNzaW9ucyByZWFkOnJlZnJlc2hfdG9rZW5zIGRlbGV0ZTpyZWZyZXNoX3Rva2VucyByZWFkOmNsaWVudF9jcmVkZW50aWFscyBjcmVhdGU6Y2xpZW50X2NyZWRlbnRpYWxzIHVwZGF0ZTpjbGllbnRfY3JlZGVudGlhbHMgZGVsZXRlOmNsaWVudF9jcmVkZW50aWFscyIsImd0eSI6ImNsaWVudC1jcmVkZW50aWFscyJ9.CsB7WFLKUbr_z53udggqZ8510XzOPszYWgzbYcZ5r85vWLSSze88q470zlWKGlW27EkQgze_T9OoV2ZKV9VJLhgRKu0g4jkpferyKkVM_8oNpnWTC_L89D7eomuR2VJCOvBcKu9bdl8BcN7Ilb6zsqsnZeA017qL_gGXAactMtTwUE2f467zv7nEoNJ5mhR-sFUBRTCyw5iBCaQxj-U1Y1wmcHPoJskSDHdqHE-CWk13Mn-68fy_94whPP0Lv_rEwA77ZB-ChdjmUdCNMJSHStaQKkiHl39_Gk-swGQ3Qt5OJMSmf49anIzPpHvqfQ6v2NMtO_n3wQ8kYqvjWDOHMw");
            Environment.SetEnvironmentVariable("AUTH0_CONNECTION_DATABASE_NAME", "Username-Password-Authentication");
            Environment.SetEnvironmentVariable("ALLOWED_DOMAINS", "isep.ipp.pt,gmail.com");
            Environment.SetEnvironmentVariable("MAX_NAME_LENGTH", "30");

            // Create the UserService instance with mocked dependencies
            userService = new UserService(unitOfWorkMock.Object, userGatewayFactory, userMapper);
        }

        [Fact]
        public void Dispose()
        {
            unitOfWorkMock.Reset();
            userRepositoryMock.Reset();
            roleRepositoryMock.Reset();
        }


        [Fact]
        public async Task AddUserAsync_SuccessfulAdditionOnAuth0_ReturnsUserResponseWithRealIamId()
        {
            // Arrange
            UserRole userRole = new("rol_yKP3qhNtC9M0lpnr", RoleType.CAMPUS_MANAGER);
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(userRole);
            var requestDto = UserDataSource.TestGatwayUserDto();

            // Act
            var response = await userService.AddUserAsync(requestDto);

            // Assert
            Assert.NotNull(response.IamId);

            // Clean up
            await DeleteUserInAuth0(response.IamId);
        }

        private static async Task DeleteUserInAuth0(string userIamId)
        {
            var userGatewayFactory = new UserGatewayFactory(new HttpClientFactory());
            var userGateway = userGatewayFactory.Create(Environment.GetEnvironmentVariable("AUTH0_DOMAIN"), Environment.GetEnvironmentVariable("AUTH0_TOKEN"));
            await userGateway.DeleteUserAsync(userIamId);
        }

        [Fact]
        public async Task AddUserAsync_UserAlreadyExistsWithSameEmailOnAuth0_ThrowsAuth0UserAlreadyExistsException()
        {
            // Arrange
            UserRole userRole = new("rol_yKP3qhNtC9M0lpnr", RoleType.CAMPUS_MANAGER);
            roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(userRole);
            var requestDto = UserDataSource.TestGatwayUserDto();

            // Act
            var response = await userService.AddUserAsync(requestDto);

            // Act, Assert
            await Assert.ThrowsAsync<Auth0UserAlreadyExistsException>(() => userService.AddUserAsync(requestDto));

            // Clean up
            await DeleteUserInAuth0(response.IamId);
        }
    }
}
