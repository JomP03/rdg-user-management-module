using Domain.Entities.Users.Role;
using System.Text.Json.Serialization;

namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the user creation
    /// </summary>
    public struct CreateManagerUserRequestDto
    {
        /// <summary>
        /// User's email
        /// </summary>
        /// <example>john.doe@isep.ipp.pt</example>
        public string Email { get; set; }

        /// <summary>
        /// User's Name
        /// </summary>
        /// <example>John Doe</example>
        public string Name { get; set; }

        /// <summary>
        /// User's Phone Number
        /// </summary>
        /// <example>915604429</example>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User's Role Id
        /// </summary>
        /// <example>0025e46a-5923-44ba-a633-78db117f47a7</example>
        public string RoleId { get; set; }

        /// <summary>
        /// User's Role instance
        /// </summary>
        [JsonIgnore]
        public UserRole? Role { get; set; }

        /// <summary>
        /// User's Password
        /// </summary>
        /// <example>mYSuperSafe123!</example>
        public string Password { get; set; }
    }
}
