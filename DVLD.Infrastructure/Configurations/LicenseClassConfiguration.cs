using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class LicenseClassConfiguration : IEntityTypeConfiguration<LicenseClass>
    {
        public void Configure(EntityTypeBuilder<LicenseClass> builder)
        {
            builder.ToTable("LicenseClasses");

            builder.HasKey(x => x.LicenseClassID);

            builder.Property(x => x.LicenseClassID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.ClassName)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.ClassDescription)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.MinimumAllowedAge)
                   .IsRequired();

            builder.Property(x => x.DefaultValidityLength)
                   .IsRequired();

            builder.Property(x => x.ClassFees)
                   .HasColumnType("smallmoney")
                   .IsRequired();
        }
    }
}