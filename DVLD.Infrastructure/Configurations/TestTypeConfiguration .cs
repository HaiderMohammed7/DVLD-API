using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Configurations
{
    public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> builder)
        {
            builder.ToTable("TestTypes");

            builder.HasKey(x => x.TestTypeID);


            builder.Property(x => x.TestTypeID)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.TestTypeTitle)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.TestTypeDescription)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.TestTypeFees)
                   .HasColumnType("smallmoney")
                   .IsRequired();
        }
    }
}