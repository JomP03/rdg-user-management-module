using App.Services.Repositories.Shared;
using Domain.Entities.SignUpRequests;
using Domain.Entities.Users;

namespace App.Services.Repositories
{
    /// <summary>
    /// Interface for the User Repository
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Find a User by its IamId
        /// </summary>
        public Task<User> GetByIamIdAsync(string iamId);

        /// <summary>
        /// Find a User by its Email
        /// </summary>
        public Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Checks if a User with the given email, nif and phone number already exists
        /// </summary>
        public Task CheckForExistingUserAsync(string email, string nif, string phoneNumber);
    }
}
