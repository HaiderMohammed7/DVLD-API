using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class LocalDrivingLicenseApplicationConfiguration : IEntityTypeConfiguration<LocalDrivingLicenseApplication>
    {
        public void Configure(EntityTypeBuilder<LocalDrivingLicenseApplication> builder)
        {
            builder.ToTable("LocalDrivingLicenseApplications");

            builder.HasKey(x => x.LocalDrivingLicenseApplicationID);

            builder.Property(x => x.LocalDrivingLicenseApplicationID)
                   .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Applications)
                   .WithMany(x => x.localDrivingLicenseApplications)
                   .HasForeignKey(x => x.ApplicationID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LicenseClass)
                   .WithMany(x => x.localDrivingLicenseApplications)
                   .HasForeignKey(x => x.LicenseClassID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}