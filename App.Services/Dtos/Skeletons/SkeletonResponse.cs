namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the skeleton response
    /// </summary>
    public struct SkeletonResponse
    {
        /// <summary>
        /// Skeleton's identification.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Skeleton's name.
        /// </summary>
        /// <example>Scary</example>
        public string Name { get; set; }
    }

    /// <summary>
    /// List of skeleton responses
    /// </summary>
    public struct SkeletonResponseList
    {
        /// <summary>
        /// Number of skeletons
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// List of skeletons
        /// </summary>
        public IEnumerable<SkeletonResponse> Skeletons { get; set; }
    }
}
