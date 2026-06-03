using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class TestAppointmentConfiguration : IEntityTypeConfiguration<TestAppointment>
    {
        public void Configure(EntityTypeBuilder<TestAppointment> builder)
        {
            builder.ToTable("TestAppointments");

            builder.HasKey(x => x.TestAppointmentID);

            builder.Property(x => x.TestAppointmentID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.AppointmentDate)
                   .HasColumnType("smalldatetime")
                   .IsRequired();

            builder.Property(x => x.PaidFees)
                   .HasColumnType("smallmoney")
                   .IsRequired();

            builder.Property(x => x.IsLocked)
                   .IsRequired();

            builder.Property(x => x.RetakeTestApplicationID)
                   .IsRequired(false);

            builder.HasOne(x => x.TestType)
                   .WithMany(x => x.TestAppointment)
                   .HasForeignKey(x => x.TestTypeID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.localDrivingLicenseApplication)
                   .WithMany(x => x.TestAppointments)
                   .HasForeignKey(x => x.LocalDrivingLicenseApplicationID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.CreatedAppointments)
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Applications)
                   .WithMany(x => x.TestAppointments)
                   .HasForeignKey(x => x.RetakeTestApplicationID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}