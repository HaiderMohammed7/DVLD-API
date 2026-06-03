using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Applications>
    {
        public void Configure(EntityTypeBuilder<Applications> builder)
        {
            builder.ToTable("Applications");

            builder.HasKey(x => x.ApplicationID);

            builder.Property(x => x.ApplicationID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.ApplicationDate)
                   .IsRequired();

            builder.Property(x => x.ApplicationStatus)
                   .IsRequired();

            builder.Property(x => x.LastStatusDate)
                   .IsRequired();

            builder.Property(x => x.PaidFees)
                   .HasColumnType("smallmoney")
                   .IsRequired();

            builder.HasOne(x => x.Person)
                   .WithMany(x => x.Applications)
                   .HasForeignKey(x => x.ApplicantPersonID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApplicationType)
                   .WithMany(x => x.Applications)
                   .HasForeignKey(x => x.ApplicationTypeID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.CreatedApplications)
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}