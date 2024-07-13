using App.Services.Repositories;
using Domain.Entities.Users.Role;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal class RoleRepository : BaseRepository<UserRole>, IRoleRepository
    {
        public RoleRepository(UMDatabaseContext context) : base(context) { }

        /// <summary>
        /// Get Manager Roles
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserRole>> GetManagerRolesAsync()
        {
            return await myContext.Roles
                .Where(role => role.Type == RoleType.CAMPUS_MANAGER ||
                               role.Type == RoleType.TASK_MANAGER ||
                               role.Type == RoleType.FLEET_MANAGER)
                .ToListAsync();
        }

        public async Task<UserRole> GetEndUserRoleAsync()
        {
            return await myContext.Roles
                .Where(role => role.Type == RoleType.ENDUSER).FirstAsync();
        }
    }
}
