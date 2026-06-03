using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class DetainedLicenseConfiguration : IEntityTypeConfiguration<DetainedLicense>
    {
        public void Configure(EntityTypeBuilder<DetainedLicense> builder)
        {
            builder.ToTable("DetainedLicenses");

            builder.HasKey(x => x.DetainID);

            builder.Property(x => x.DetainID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.DetainDate)
                   .IsRequired();

            builder.Property(x => x.FineFees)
                   .HasColumnType("smallmoney")
                   .IsRequired();

            builder.Property(x => x.IsReleased)
                   .IsRequired();

            builder.Property(x => x.ReleaseDate)
                   .IsRequired(false);

            builder.HasOne(x => x.License)
                   .WithMany(x => x.DetainedLicenses)
                   .HasForeignKey(x => x.LicenseID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.CreatedDetainedLicenses)
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.CreatedDetainedLicenses)
                   .HasForeignKey(x => x.ReleasedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Applications>()
                   .WithMany()
                   .HasForeignKey(x => x.ReleaseApplicationID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}