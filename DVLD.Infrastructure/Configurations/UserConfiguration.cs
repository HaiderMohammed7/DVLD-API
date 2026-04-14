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

            builder.Property(x => x.UserName)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasOne<Person>()
                   .WithMany()
                   .HasForeignKey(x => x.PersonID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}