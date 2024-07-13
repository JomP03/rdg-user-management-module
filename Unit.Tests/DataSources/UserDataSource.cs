using Domain.Entities.Users.Role;
using Domain.Entities.Users;
using IAM.Models;
using App.Services.Dtos;

namespace Unit.Tests.DataSources
{
    public class UserDataSource
    {
        private static readonly UserRole CampusManagerRole = new("someIamId", RoleType.CAMPUS_MANAGER);
        private static readonly UserRole FleetManagerRole = new("someIamId", RoleType.FLEET_MANAGER);
        private static readonly UserRole TaskManagerRole = new("someIamId", RoleType.TASK_MANAGER);
        private static readonly UserRole EndUserRole = new("someIamId", RoleType.ENDUSER);

        public static User CampusManagerA()
        {
            return CreateUser("campusmanagerA@gmail.com", "Campus Manager A", "933756104", "Password123!", CampusManagerRole);
        }

        public static CreateManagerUserRequestDto CampusManagerARequestDto()
        {
            return new CreateManagerUserRequestDto
            {
                Email = "campusmanagerA@gmail.com",
                Name = "Campus Manager A",
                PhoneNumber = "933756104",
                Password = "Password123!",
                RoleId = "someRoleId"
            };
        }

        public static UserResponse CampusManagerAResponse()
        {
            return new UserResponse
            {
                Email = "campusmanagerA@gmail.com",
                Name = "Campus Manager A",
                PhoneNumber = "933756104",
                RoleName = RoleType.CAMPUS_MANAGER.ToString(),
            };
        }

        public static CreateManagerUserRequestDto TestGatwayUserDto()
        {
            var randomNumber = new Random();

            return new CreateManagerUserRequestDto
            {
                Email = "intTest@gmail.com",
                Name = "Integration Test",
                PhoneNumber = "911687325",
                Password = "Password123!",
                RoleId = "someRoleId"
            };
        }

        public static User CampusManagerB()
        {
            return CreateUser("campusmanagerB@gmail.com", "Campus Manager B", "987654321", "SecurePass!456", CampusManagerRole);
        }

        public static CreateManagerUserRequestDto CampusManagerBRequestDto()
        {
            return new CreateManagerUserRequestDto
            {
                Email = "campusmanagerB@gmail.com",
                Name = "Campus Manager B",
                PhoneNumber = "987654321",
                Password = "SecurePass!456",
                RoleId = "someRoleId"
            };
        }



        public static User FleetManagerA()
        {
            return CreateUser("fleetmanagerA@gmail.com", "Fleet Manager A", "913756104", "FleetPass123!", FleetManagerRole);
        }

        public static User FleetManagerB()
        {
            return CreateUser("fleetmanagerB@gmail.com", "Fleet Manager B", "963756104", "SecureFleet!456", FleetManagerRole);
        }

        public static User TaskManagerA()
        {
            return CreateUser("taskmanagerA@gmail.com", "Task Manager A", "351963756104", "TaskPass123!", TaskManagerRole);
        }

        public static User TaskManagerB()
        {
            return CreateUser("taskmanagerB@gmail.com", "Task Manager B", "351933756104", "SecureTask!456", TaskManagerRole);
        }

        public static User EndUserA()
        {
            return CreateUserWithNif("enduserA@gmail.com", "End User A", "960443528", "EndPass123!", EndUserRole, "535813244");
        }

        public static User EndUserB()
        {
            return CreateUserWithNif("enduserB@gmail.com", "End User B", "919656784", "SecureEnd!456", EndUserRole, "453777279");
        }

        private static User CreateUser(string email, string name, string phoneNumber, string password, UserRole role)
        {
            return new User(email, name, phoneNumber, role, null, password);
        }

        private static User CreateUserWithNif(string email, string name, string phoneNumber, string password, UserRole role, string nif)
        {
            return new User(email, name, phoneNumber, role, nif, password);
        }

        public static Auth0UserResponse Auth0UserResponse(string iamIdToBeUsed)
        {
            return new Auth0UserResponse
            {
                UserId = iamIdToBeUsed,
            };
        }

        public static UserResponse UserResponseA()
        {
            return MapFromDomainToOutDto(CampusManagerA());
        }


        private static UserResponse MapFromDomainToOutDto(User domain)
        {
            var outDto = new UserResponse
            {
                Id = domain.Id.ToString(),
                Name = domain.Name.Value,
                Email = domain.Email.Value,
                PhoneNumber = domain.PhoneNumber.Value,
                RoleName = domain.Role.Type.ToString(),
                IamId = domain.IamId,
            };

            if (domain.Role.Type == RoleType.ENDUSER)
            {
                outDto.Nif = domain.Nif.Value;
            }

            return outDto;

        }


    }
}
