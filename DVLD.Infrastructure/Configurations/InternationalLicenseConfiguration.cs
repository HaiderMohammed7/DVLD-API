using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class InternationalLicenseConfiguration : IEntityTypeConfiguration<InternationalLicense>
    {
        public void Configure(EntityTypeBuilder<InternationalLicense> builder)
        {
            builder.ToTable("InternationalLicenses");

            builder.HasKey(x => x.InternationalLicenseID);

            builder.Property(x => x.InternationalLicenseID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.IssueDate)
                   .HasColumnType("smalldatetime")
                   .IsRequired();

            builder.Property(x => x.ExpirationDate)
                   .HasColumnType("smalldatetime")
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasOne(x => x.Applications)
                   .WithMany(x => x.InternationalLicenses)
                   .HasForeignKey(x => x.ApplicationID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Driver)
                   .WithMany(x => x.InternationalLicenses)
                   .HasForeignKey(x => x.DriverID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.License)
                   .WithMany(x => x.InternationalLicenses)
                   .HasForeignKey(x => x.IssuedUsingLocalLicenseID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.CreatedInternationalLicenses)
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}