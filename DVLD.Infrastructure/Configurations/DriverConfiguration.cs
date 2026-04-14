using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Drivers");

            builder.HasKey(x => x.DriverID);

            builder.Property(x => x.DriverID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedDate)
                   .HasColumnType("smalldatetime")
                   .IsRequired();

            builder.HasOne<Person>()
                   .WithMany()
                   .HasForeignKey(x => x.PersonID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}