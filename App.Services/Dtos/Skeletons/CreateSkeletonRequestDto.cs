namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the skeleton creation
    /// </summary>
    public struct CreateSkeletonRequestDto
    {
        /// <summary>
        /// Skeleton Name
        /// </summary>
        /// <example>Scary</example>
        public string Name { get; set; }
    }
}
