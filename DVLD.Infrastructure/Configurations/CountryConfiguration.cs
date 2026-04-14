using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {

            builder.ToTable("Countries");

            builder.HasKey(x => x.CountryID);

            builder.Property(x => x.CountryID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.CountryName)
                   .HasMaxLength(50)
                   .IsRequired();
        }
    }
}