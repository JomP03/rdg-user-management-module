using System.Threading.Tasks;
using IAM.Models;

namespace IAM.Gateways.Users
{
    /// <summary>
    /// Interface for the gateway for interacting with the Auth0 Users API.
    /// </summary>
    public interface IUsersGateway
    {
        /// <summary>
        /// Creates a User in Auth0.
        /// </summary>
        /// <param name="user">User to be created.</param>
        /// <returns>User Response.</returns>
        Task<Auth0UserResponse> CreateUserAsync(Auth0User user);

        /// <summary>
        /// Adds a role to a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        Task AddRoleToUserAsync(string userId, string roleId);

        /// <summary>
        /// Deletes a user from Auth0.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUserAsync(string userId);

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task RemoveUserRoleAsync(string userId, string roleId);
    }
}