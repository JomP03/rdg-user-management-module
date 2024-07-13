using Domain.Entities.Users;
using Domain.Entities.Users.Role;
using Domain.Shared;
using System.Data;
using Unit.Tests.DataSources;

namespace Unit.Tests.Domain.UserTests
{
    [Collection("UserTests")]
    public class UserTests
    {

        // Instanciate the roles
        private readonly UserRole endUserRole = new("someIamId", RoleType.ENDUSER);
        private readonly UserRole[] managerRoles =
        {
            new("someIamId", RoleType.CAMPUS_MANAGER),
            new("someIamId", RoleType.FLEET_MANAGER),
            new("someIamId", RoleType.TASK_MANAGER)
        };

        public UserTests()
        {
            Environment.SetEnvironmentVariable("ALLOWED_DOMAINS", "isep.ipp.pt,gmail.com");
            Environment.SetEnvironmentVariable("MAX_NAME_LENGTH", "20");
        }

        [Theory]
        [InlineData("teste@isep.ipp.pt", "John Doe", "913756104", "ValidPass123!")]
        [InlineData("teste@gmail.com", "Mary Jean", "923756104", "SuperSafe1!89")]
        public void Constructor_ValidManagersCases_ShouldCreateUserName(string email, string name, string phoneNumber, string password)
        {
            // Arrange
            UserRole role = managerRoles[new Random().Next(0, managerRoles.Length)];

            // Act
            User user = new(email, name, phoneNumber, role, null, password);
            
            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email.Value);
            Assert.Equal(name, user.Name.Value);
            Assert.Equal(phoneNumber, user.PhoneNumber.Value);
            Assert.Equal(password, user.Password.Value);
            Assert.Equal(role, user.Role);
        }

        [Theory]
        [InlineData("teste@isep.ipp.pt", "John Doe", "913756104", "ValidPass123!", "172658462")]
        [InlineData("teste@gmail.com", "Mary Jane", "923756104", "SuperSafe1!89", "679082433")]
        public void Constructor_ValidEndUserCases_ShouldCreateUserName(string email, string name, string phoneNumber, string password, string nif)
        {
            // Act
            User user = new(email, name, phoneNumber, endUserRole, nif, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email.Value);
            Assert.Equal(name, user.Name.Value);
            Assert.Equal(phoneNumber, user.PhoneNumber.Value);
            Assert.Equal(password, user.Password.Value);
            Assert.Equal(endUserRole, user.Role);
            Assert.Equal(nif, user.Nif.Value);
        }

        [Fact]
        public void Constructor_InvalidNifWhenEndUser_ShouldThrowArgumentNullException()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new User("teste@isep.ipp.pt", "John Doe", "913756104", endUserRole, null, "ValidPass123!"));
        }

        [Theory]
        [InlineData("teste@isep.ipp.pt", "John Doe", "13756104", "ValidPass123!")]
        [InlineData("teste@gmail.co", "Mary Jean", "923756104", "SuperSafe1!89")]
        [InlineData("teste@gmail.com", "Mary Jean", "923756104", "SuperSafe189")]
        [InlineData("teste@isep.ipp.pt", "John 1Doe", "913756104", "ValidPass123!")]
        public void Constructor_InvalidManagerCases_ShouldThrowBusinessRuleValidationException(string email, string name, string phoneNumber, string password)
        {
            // Arrange
            UserRole role = managerRoles[new Random().Next(0, managerRoles.Length)];

            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => new User(email, name, phoneNumber, role, null, password));
        }

        [Theory]
        [InlineData("teste@isep.pt", "John Doe", "913756104", "ValidPass123!", "172658462")]
        [InlineData("teste@gmail.com", "Mary1 Jane", "923756104", "SuperSafe1!89", "679082433")]
        [InlineData("teste@isep.ipp.pt", "John Doe", "913756104", "ValidPass123!", "172658461")]
        [InlineData("teste@isep.ipp.pt", "John Doe", "213756104", "ValidPass123!", "172658462")]
        [InlineData("teste@isep.ipp.pt", "John Doe", "913756104", "ValidPass123", "172658462")]
        public void Constructor_InvalidEndUserCases_ShouldThrowBusinessRuleValidationException(string email, string name, string phoneNumber, string password, string nif)
        {
            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => new User(email, name, phoneNumber, endUserRole, nif, password));
        }

        [Fact]
        public void AddIamId_ValidCase_ShouldAddIamIdToUser()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();
            var iamId = "someIamId";

            // Act
            user.AddIamId(iamId);

            // Assert
            Assert.Equal(iamId, user.IamId);
        }

        [Fact]
        public void AddIamId_NullIamId_ShouldThrowArgumentNullException()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => user.AddIamId(null));
        }

        [Fact]
        public void UpdateName_ValidCase_ShouldUpdateUserName()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();
            var newName = "John Doe";

            // Act
            user.UpdateName(newName);

            // Assert
            Assert.Equal(newName, user.Name.Value);
        }

        [Fact]
        public void UpdateName_InvalidCase_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();
            var newName = "John Doe !!!";

            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => user.UpdateName(newName));
        }

        [Fact]
        public void UpdateName_NullName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => user.UpdateName(null));
        }

        [Fact]
        public void UpdatePhoneNumber_ValidCase_ShouldUpdateUserPhoneNumber()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();
            var newPhoneNumber = "913756104";

            // Act
            user.UpdatePhoneNumber(newPhoneNumber);

            // Assert
            Assert.Equal(newPhoneNumber, user.PhoneNumber.Value);
        }

        [Fact]
        public void UpdatePhoneNumber_InvalidCase_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();
            var newPhoneNumber = "9137561045";

            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => user.UpdatePhoneNumber(newPhoneNumber));
        }

        [Fact]
        public void UpdatePhoneNumber_NullPhoneNumber_ShouldThrowArgumentNullException()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => user.UpdatePhoneNumber(null));
        }

        [Fact]
        public void UpdateNif_ValidCase_ShouldUpdateUserNif()
        {

            // Arrange
            var user = UserDataSource.EndUserA();
            var newNif = "453777279";

            // Act
            user.UpdateNif(newNif);

            // Assert
            Assert.Equal(newNif, user.Nif.Value);

        }

        [Fact]
        public void UpdateNif_InvalidCase_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var user = UserDataSource.EndUserA();
            var newNif = "53581";

            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => user.UpdateNif(newNif));
        }

        [Fact]
        public void UpdateNif_NullNif_ShouldThrowArgumentNullException()
        {
            // Arrange
            var user = UserDataSource.EndUserA();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => user.UpdateNif(null));
        }

        [Fact]
        public void UpdateNif_NotEndUser_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var user = UserDataSource.CampusManagerA();
            var newNif = "535813200";

            // Act, Assert
            Assert.Throws<BusinessRuleValidationException>(() => user.UpdateNif(newNif));
        }

    }
}
