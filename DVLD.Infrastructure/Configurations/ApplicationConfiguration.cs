using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
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

            builder.HasOne<Person>()
                   .WithMany()
                   .HasForeignKey(x => x.ApplicantPersonID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationType>()
                   .WithMany()
                   .HasForeignKey(x => x.ApplicationTypeID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}