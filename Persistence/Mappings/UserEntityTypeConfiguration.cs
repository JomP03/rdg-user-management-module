using Domain.Entities.Users;
using Domain.Entities.Users.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Mappings
{

    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.Id);

            // Add the email as a unique and required column
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired();
                email.HasIndex(e => e.Value).IsUnique();
            });

            // Add the phoneNumber as a unique and required column
            builder.OwnsOne(u => u.PhoneNumber, phoneNumber =>
            {
                phoneNumber.Property(p => p.Value)
                    .HasColumnName("PhoneNumber")
                    .IsRequired();
                phoneNumber.HasIndex(p => p.Value).IsUnique();
            });

            // Add the name as a required column
            builder.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired();
            });

            // Add the iamId as a unique and required column
            builder.Property(s => s.IamId).HasColumnName("IamId").IsRequired();
            builder.HasIndex(s => s.IamId).IsUnique();

            // Add the nif as a unique and optional column
            builder.OwnsOne(u => u.Nif, nif =>
            {
                nif.Property(n => n.Value)
                    .HasColumnName("Nif");
                nif.HasIndex(n => n.Value).IsUnique();
            });

            builder.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            // Don't store the password in the database
            builder.Ignore(s => s.Password);
        }
    }
}
