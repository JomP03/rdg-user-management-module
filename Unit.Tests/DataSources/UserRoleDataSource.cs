using Domain.Entities.Users.Role;

namespace Unit.Tests.DataSources
{
    public class UserRoleDataSource
    {
        public static UserRole CampusManagerRole()
        {
            return new("someIamId", RoleType.CAMPUS_MANAGER);
        }

        public static UserRole FleetManagerRole()
        {
            return new("someIamId", RoleType.FLEET_MANAGER);
        }

        public static UserRole TaskManagerRole()
        {
            return new("someIamId", RoleType.TASK_MANAGER);
        }

        public static UserRole EndUserRole()
        {
            return new("someIamId", RoleType.ENDUSER);
        }
    }
}
