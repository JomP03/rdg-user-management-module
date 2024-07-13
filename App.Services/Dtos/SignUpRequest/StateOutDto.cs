

namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the response of the state of a Sign Up Request
    /// </summary>
    public struct StateOutDto
    {
        /// <summary>
        /// Approved, Rejected or Requested
        /// </summary>
        /// <example>true</example>
        public string State { get; set; }


    }
}