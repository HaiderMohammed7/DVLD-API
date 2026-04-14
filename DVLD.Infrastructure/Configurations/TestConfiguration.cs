using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Tests");

            builder.HasKey(x => x.TestID);


            builder.Property(x => x.TestID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.TestResult)
                   .IsRequired();

            builder.Property(x => x.Notes)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.HasOne<TestAppointment>()
                   .WithMany()
                   .HasForeignKey(x => x.TestAppointmentID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}