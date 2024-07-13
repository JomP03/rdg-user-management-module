namespace App.Services.Dtos
{
    /// <summary>
    /// Descrição do tipo UserInDTO.
    /// </summary>
    public struct CreateSignUpRequestRequestDto
    {
        /// <summary>
        /// External ID from the IAM
        /// </summary>
        /// <example>Iam_ID</example>
        public string IamId { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        /// <example>pedro@isep.ipp.pt</example>
        public string Email { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        /// <example>Pedro</example>
        public string Name { get; set; }

        /// <summary>
        /// User Phone Number
        /// </summary> 
        /// <example>932782461</example>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User Nif
        /// </summary>
        /// <example>247677949</example>
        public string Nif { get; set; }
    }
}
