namespace App.Services.Dtos
{
    /// <summary>
    /// Descrição do tipo AuthResponseDto.
    /// </summary>
    public struct AuthResponseDto
    {
        /// <summary>
        /// External Id from the IAM
        /// </summary>
        public string IamId { get; set; }

        /// <summary>
        /// User's Role.
        /// </summary>
        public string Role { get; set; }


    }

    /// <summary>
    /// List of user responses
    /// </summary>
    public struct AuthResponseList
    {
        /// <summary>
        /// Number of users
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// List of users
        /// </summary>
        public IEnumerable<AuthResponseDto> Auth { get; set; }
    }

}
