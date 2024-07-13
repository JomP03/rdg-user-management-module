namespace App.Services.Dtos
{
    /// <summary>
    /// Descrição do tipo SignUp Request.
    /// </summary>
    public struct SignUpRequestResponseDto
    {
        /// <summary>
        /// External Id from the IAM
        /// </summary>
        public string IamId { get; set; }

        /// <summary>
        /// User's identification.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User Phone Number
        /// </summary> 
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User Nif
        /// </summary>
        public string Nif { get; set; }

        /// <summary>
        /// Sign Up Request Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Creation Time of the Sign Up Request
        /// </summary>
        public string CreationTime { get; set; }

        /// <summary>
        /// Time of the action of approving or rejecting the Sign Up Request
        /// </summary>
        public string ActionTime { get; set; }

        /// <summary>
        /// User that approved or rejected the Sign Up Request
        /// </summary>
        public string ActionedBy { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct RegistrationRequestResponseList
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SignUpRequestResponseDto> RegistrationRequests { get; set; }
    }
}
