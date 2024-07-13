namespace App.Services.Repositories.Shared
{
    /// <summary>
    /// Interface for the Unit of Work
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Repository for the Skeletons
        /// </summary>
        ISkeletonRepository Skeletons { get; }

        /// <summary>
        /// Repository for the Users
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Repository for the Roles
        /// </summary>
        IRoleRepository Roles { get; }

        ISignUpRequestRepository SignUpRequests { get; }


        /// <summary>
        /// Commits the changes to the database from the Repository
        /// </summary>
        Task<int> CommitAsync();
    }

}
