using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class ApplicationTypeConfiguration : IEntityTypeConfiguration<ApplicationType>
    {
        public void Configure(EntityTypeBuilder<ApplicationType> builder)
        {
            builder.ToTable("ApplicationTypes");

            builder.HasKey(x => x.ApplicationTypeID);

            builder.Property(x => x.ApplicationTypeID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.ApplicationTypeTitle)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(x => x.ApplicationFees)
                   .HasColumnType("smallmoney")
                   .IsRequired();
        }
    }
}