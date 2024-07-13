using Domain.Shared;
using System;

namespace Domain.Entities.Users.Role
{
    /// <summary>
    /// Entity that represents a user's role
    /// </summary>
    public class UserRole : Entity
    {
        public RoleType Type { get; private set; }
        public string IamId { get; private set; }

        protected UserRole()
        {
            // Required by EF!!!
        }

        /// <summary>
        /// Creates a UserRole Entity
        /// </summary>
        /// <param name="iamId"></param>
        /// <param name="roleType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserRole(string iamId, RoleType roleType)
        {
            // Check if roleId is null or empty
            if (string.IsNullOrEmpty(iamId))
            {
                throw new ArgumentNullException(nameof(iamId));
            }

            Type = roleType;
            IamId = iamId;
        }
    }
}