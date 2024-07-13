using App.Services.Repositories;
using Domain.Entities.Skeleton;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories
{
    internal class SkeletonRepository : BaseRepository<Skeleton>, ISkeletonRepository
    {
        public SkeletonRepository(UMDatabaseContext context) : base(context)
        {
        }
    }
}
