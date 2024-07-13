using App.Services.Repositories;
using App.Services.Repositories.Shared;
using System;
using System.Threading.Tasks;

namespace Persistence.Repositories.Shared
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly UMDatabaseContext myContext;

        private ISkeletonRepository mySkeletons;

        private IUserRepository myUsers;
        private IRoleRepository myRoles;

        private ISignUpRequestRepository mySignUpRequest;

        /// <summary>
        /// Initializes a UnitOfWork
        /// </summary>
        /// <param name="context">Database context</param>
        public UnitOfWork(UMDatabaseContext context)
        {
            myContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ISkeletonRepository Skeletons
        {
            get
            {
                return mySkeletons ??= new SkeletonRepository(myContext);
            }
        }

        public IUserRepository Users
        {
            get
            {
                return myUsers ??= new UserRepository(myContext);
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                return myRoles ??= new RoleRepository(myContext);
            }
        }

        public ISignUpRequestRepository SignUpRequests
        {
            get
            {
                return mySignUpRequest ??= new SignUpRequestRepository(myContext);
            }
        }

        public async Task<int> CommitAsync()
        {
            return await myContext.SaveChangesAsync();
        }

    }

}
