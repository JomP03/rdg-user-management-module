using System.Collections.Generic;

namespace IAM.Models
{
    /// <summary>
    /// Model for creating a user in Auth0.
    /// </summary>
    public struct Auth0User
    {
        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Name of the database connection where the user will be created.
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Prevents from sending a verification email when creating a user.
        /// </summary>
        public bool VerifyEmail { get; set; }
    }

    public struct Auth0UserResponse
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Blocked { get; set; }
        public bool? EmailVerified { get; set; }
        public bool? PhoneVerified { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool? VerifyEmail { get; set; }
        public string Username { get; set; }
        public Dictionary<string, string> AppMetadata { get; set; }
        public Dictionary<string, string> UserMetadata { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public IEnumerable<Identity> Identities { get; set; }
        public IEnumerable<string> Multifactor { get; set; }
        public string LastIp { get; set; }
        public string LastLogin { get; set; }
        public int LoginsCount { get; set; }
    }
    public struct Identity
    {
        public string Connection { get; set; }
        public string UserId { get; set; }
        public string Provider { get; set; }
        public bool IsSocial { get; set; }
    }
}
