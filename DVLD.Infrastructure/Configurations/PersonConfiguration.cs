using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(x => x.PersonID);

            builder.Property(x => x.PersonID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.NationalNo)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.FirstName)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.SecondName)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.ThirdName)
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.Property(x => x.LastName)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.DateOfBirth)
                   .HasColumnType("datetime")
                   .IsRequired();

            builder.Property(x => x.Gendor)
                   .IsRequired();

            builder.Property(x => x.Address)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.Phone)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(x => x.ImagePath)
                   .HasMaxLength(250)
                   .IsRequired(false);

            builder.HasOne<Country>()
                   .WithMany()
                   .HasForeignKey(x => x.NationalityCountryID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}