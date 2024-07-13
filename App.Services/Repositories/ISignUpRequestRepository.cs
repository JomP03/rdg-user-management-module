using App.Services.Repositories.Shared;
using Domain.Entities.SignUpRequests;
using Domain.Entities.Skeleton;

namespace App.Services.Repositories
{
    public interface ISignUpRequestRepository : IRepository<SignUpRequest>
    {

        /// <summary>
        /// Find a SignUpRequest by its IamId
        /// </summary>
        public Task<SignUpRequest> GetByIamIdAsync(string iamId);

        /// <summary>
        /// Find SignUpRequests by their state
        /// </summary>
        public Task<List<SignUpRequest>> GetByStateAsync(SignUpRequestStatus state);

        /// <summary>
        /// Find SignUpRequests by their state and IamId
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns></returns>
        public Task<List<SignUpRequest>> GetAllByIamIdAsync(string iamId);

    }
}
