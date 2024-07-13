using Domain.Entities.Skeleton;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Mappings
{

    public class SkeletonEntityTypeConfiguration : IEntityTypeConfiguration<Skeleton>
    {
        public void Configure(EntityTypeBuilder<Skeleton> builder)
        {
            builder.HasKey(b => b.Id);

            // Add the name as a required column
            builder.OwnsOne(s => s.Name).Property(p => p.Value).HasColumnName("Name").IsRequired();
        }
    }
}
