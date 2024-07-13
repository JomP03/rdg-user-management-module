using Domain.Entities.Users.Role;
using System.Text.Json.Serialization;

namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the request of the action of approving or rejecting a Sign Up Request
    /// </summary>
    public struct SignUpRequestActionDto
    {
        /// <summary>
        /// Approved or Rejected
        /// </summary>
        /// <example>true</example>
        public bool Action { get; set; }

        /// <summary>
        /// Comment of the action
        /// </summary>
        /// <example>Approved because its a student.</example>
        public string Comment { get; set; }

        /// <summary>
        /// IamId of the user that is approving or rejecting the Sign Up Request
        /// </summary>
        /// <example>1</example>
        public string IamId { get; set; }
    }
}
