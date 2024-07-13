using Domain.Entities.SignUpRequests;
using Domain.Entities.Skeleton;
using Domain.Entities.Users;
using Domain.Entities.Users.Role;
using Microsoft.EntityFrameworkCore;
using Persistence.Mappings;

namespace Persistence
{
    public class UMDatabaseContext : DbContext
    {
        public ModelBuilder myModelBuilder;
        public UMDatabaseContext(DbContextOptions<UMDatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Skeleton> Skeletons { get; private set; }
        public virtual DbSet<User> Users { get; private set; }
        public virtual DbSet<UserRole> Roles { get; private set; }

        public virtual DbSet<SignUpRequest> SignUpRequests { get; private set; }


        /// <summary>
        /// Configures the entities to add to the database to its respective columns
        /// </summary>
        /// <param name="modelBuilder">Database builder</param>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            myModelBuilder = modelBuilder;
            modelBuilder.ApplyConfiguration(new SkeletonEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SignUpRequestEntityTypeConfiguration());
        }
    }
}
