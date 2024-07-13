using App.Services.Dtos;

namespace App.Services.Services.Skeletons
{
    /// <summary>
    /// Interface for the Skeletons service
    /// </summary>
    public interface ISkeletonService
    {
        /// <summary>
        /// Add a Skeleton to database
        /// </summary>
        /// <param name="createSkeletonDto">dto of the skeleton to be added</param>
        Task<SkeletonResponse> AddSkeletonAsync(CreateSkeletonRequestDto createSkeletonDto);

        /// <summary>
        /// Gets a Skeleton by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The skeleton response with the matching id</returns>
        Task<SkeletonResponse> GetByIdAsync(string id);

        /// <summary>
        /// Gets all skeletons
        /// </summary>
        /// <returns>The list of skeletons</returns>
        Task<SkeletonResponseList> GetSkeletonsAsync();
    }
}
