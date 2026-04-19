using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.UserID);

            builder.Property(x => x.UserID)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.AuthUserId)
                .IsRequired();

            builder.HasIndex(x => x.AuthUserId)
                .IsUnique();

            builder.Property(x => x.PersonID)
                .IsRequired();

            builder.HasIndex(x => x.PersonID)
                .IsUnique();

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne<Person>()
                .WithOne()
                .HasForeignKey<User>(x => x.PersonID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}