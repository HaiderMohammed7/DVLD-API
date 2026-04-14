using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class LicenseConfiguration : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)
        {
            builder.ToTable("Licenses");

            builder.HasKey(x => x.LicenseID);

            builder.Property(x => x.LicenseID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.IssueDate)
                   .HasColumnType("datetime")
                   .IsRequired();

            builder.Property(x => x.ExpirationDate)
                   .HasColumnType("datetime")
                   .IsRequired();

            builder.Property(x => x.Notes)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.PaidFees)
                   .HasColumnType("smallmoney")
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.Property(x => x.IssueReason)
                   .IsRequired();

            builder.HasOne<Application>()
                   .WithMany()
                   .HasForeignKey(x => x.ApplicationID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Driver>()
                   .WithMany()
                   .HasForeignKey(x => x.DriverID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<LicenseClass>()
                   .WithMany()
                   .HasForeignKey(x => x.LicenseClass)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}