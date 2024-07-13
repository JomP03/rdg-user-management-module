using Domain.Entities.Users.Role;
using System.Text.Json.Serialization;

namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the user update
    /// </summary>
    public struct UpdateUserRequestDto
    {
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
        /// User's Nif
        /// </summary>
        /// <example>244969450</example>
        public string Nif { get; set; }
    }
}
