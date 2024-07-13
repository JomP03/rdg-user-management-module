namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the User response
    /// </summary>
    public struct UserResponse
    {
        /// <summary>
        /// User's identification.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User's name.
        /// </summary>
        /// <example>Scary</example>
        public string Name { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's nif.
        /// </summary>
        public string Nif { get; set; }

        /// <summary>
        /// User's phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User's role name.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// User's IamId.
        /// </summary>
        public string IamId { get; set; }
    }

    /// <summary>
    /// List of user responses
    /// </summary>
    public struct UserResponseList
    {
        /// <summary>
        /// List of users
        /// </summary>
        public IEnumerable<UserResponse> Users { get; set; }
    }
}
