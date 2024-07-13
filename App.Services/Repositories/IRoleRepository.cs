using App.Services.Repositories.Shared;
using Domain.Entities.Users.Role;

namespace App.Services.Repositories
{
    /// <summary>
    /// Interface for the Role Repository
    /// </summary>
    public interface IRoleRepository : IRepository<UserRole>
    {
        /// <summary>
        /// Get Manager Roles
        /// </summary>
        /// <returns></returns>
        Task<List<UserRole>> GetManagerRolesAsync();


        /// <summary>
        /// Get EndUser Role
        /// </summary>
        /// <returns></returns>
        Task<UserRole> GetEndUserRoleAsync();
    }
}
 