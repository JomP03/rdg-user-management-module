using App.Services.Dtos;
using App.Services.Mappers;
using App.Services.Repositories;
using App.Services.Repositories.Shared;

namespace App.Services.Services.Skeletons
{
    /// <summary>
    /// Service for the Skeletons
    /// </summary>
    public class SkeletonService : ISkeletonService
    {

        private readonly IUnitOfWork myUnitOfWork;
        private readonly ISkeletonRepository mySkeletonRepository;

        /// <summary>
        /// Initializes a SkeletonService
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SkeletonService(IUnitOfWork unitOfWork)
        {
            myUnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            mySkeletonRepository = unitOfWork.Skeletons;
        }

        /// <summary>
        /// Add a Skeleton to database
        /// </summary>
        /// <param name="createSkeletonDto">The skeleton to be added</param>
        public async Task<SkeletonResponse> AddSkeletonAsync(CreateSkeletonRequestDto createSkeletonDto)
        {
            // Map the skeleton dto into a skeleton domain
            var skeletonMapper = new SkeletonMapper();
            var skeleton = skeletonMapper.MapFromCreateDtoToDomain(createSkeletonDto);

            // Add the skeleton to the database
            await mySkeletonRepository.AddAsync(skeleton);
            await myUnitOfWork.CommitAsync();

            // Map the skeleton into a response dto and return it
            var response = skeletonMapper.MapFromDomainToOutDto(skeleton);
            return response;
        }

        /// <summary>
        /// Gets a Skeleton by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<SkeletonResponse> GetByIdAsync(string id)
        {
            // Do not use this exception on get by id's. I just put it here to show how to use it. It should be used when you are performing a query and you expect to get one entity, but you get none.
            var skeleton = await mySkeletonRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("Skeleton", "id", id);

            // Map the skeleton into a out dto
            var skeletonMapper = new SkeletonMapper();
            var response = skeletonMapper.MapFromDomainToOutDto(skeleton);

            // Return the skeleton
            return response;
        }

        /// <summary>
        /// Gets all skeletons
        /// </summary>
        /// <returns></returns>
        public async Task<SkeletonResponseList> GetSkeletonsAsync()
        {
            var skeletons = await mySkeletonRepository.GetAllAsync();

            // Get the number of skeletons retrieved
            var count = skeletons.Count;

            // Instantiate the mapper
            var skeletonMapper = new SkeletonMapper();

            // Create the response
            var response = new SkeletonResponseList
            {
                Count = count,
                Skeletons = skeletons.Select(s => skeletonMapper.MapFromDomainToOutDto(s))
            };

            return response;
        }
    }
}
