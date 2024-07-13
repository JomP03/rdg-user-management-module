using Domain.Entities.SignUpRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Mappings
{

    public class SignUpRequestEntityTypeConfiguration : IEntityTypeConfiguration<SignUpRequest>
    {
        public void Configure(EntityTypeBuilder<SignUpRequest> builder)
        {
            builder.HasKey(b => b.Id);

            // Add the name as a required column
            builder.OwnsOne(s => s.Name, name =>
            {
                name.Property(p => p.Value)
                    .HasColumnName("Name")
                    .IsRequired();
            });

            // Add the email as a unique and optional column
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(n => n.Value)
                    .HasColumnName("Email");
            });

            // Add the phoneNumber as a unique and required column
            builder.OwnsOne(s => s.PhoneNumber, phoneNumber =>
            {
                phoneNumber.Property(p => p.Value)
                    .HasColumnName("PhoneNumber")
                    .IsRequired();
            });

            // Add the nif as a unique and optional column
            builder.OwnsOne(u => u.Nif, nif =>
            {
                nif.Property(n => n.Value)
                    .HasColumnName("Nif");
            });

            // Add the iamId as a unique and required column
            builder.Property(s => s.IamId).HasColumnName("IamId").IsRequired();

            // Add the status as a required column
            builder.Property(s => s.Status).HasConversion<string>().IsRequired();

            // Add the creation Time as a required column
            builder.Property(s => s.CreationTime).IsRequired();

            // Add the action Time 
            builder.Property(s => s.ActionTime);

            // Add the actioned by (who did the action) 
            builder.HasOne(s => s.ActionedBy);

            // Add the action comment (why did the action)
            builder.Property(s => s.ActionComment);
        }
    }
}
