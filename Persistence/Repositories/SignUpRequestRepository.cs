using App.Services.Repositories;
using Domain.Entities.SignUpRequests;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class SignUpRequestRepository : BaseRepository<SignUpRequest>, ISignUpRequestRepository
    {
        public SignUpRequestRepository(UMDatabaseContext Context) : base(Context)
        {
        }

        public async Task<SignUpRequest> GetByIamIdAsync(string iamId)
        {
            return await myContext.SignUpRequests.FirstOrDefaultAsync(s => s.IamId == iamId);
        }

        public async Task<List<SignUpRequest>> GetByStateAsync(SignUpRequestStatus state)
        {
            return await myContext.SignUpRequests.Where(s => s.Status == state).ToListAsync();
        }

        public Task<List<SignUpRequest>> GetAllByIamIdAsync(string iamId)
        {
            return myContext.SignUpRequests.Where(s => s.IamId == iamId).ToListAsync();
        }
    }
}
