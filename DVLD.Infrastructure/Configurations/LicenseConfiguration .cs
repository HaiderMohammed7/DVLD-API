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

            builder.HasOne(x => x.Applications)
                   .WithMany(x => x.Licenses)
                   .HasForeignKey(x => x.ApplicationID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Driver)
                   .WithMany(x => x.Licenses)
                   .HasForeignKey(x => x.DriverID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Classes)
                   .WithMany(x => x.Licenses)
                   .HasForeignKey(x => x.LicenseClass)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.CreatedLicenses)
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}