using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Data
{
    public class DVLDDbContext : DbContext
    {
        public DVLDDbContext(DbContextOptions<DVLDDbContext> options) : base(options)
        {
        }

        public DbSet<Applications> Applications { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DetainedLicense> DetainedLicense { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<InternationalLicense> InternationalLicenses { get; set; }
        public DbSet<LicenseClass> LicenseClass { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<TestAppointment> TestAppointments { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DVLDDbContext).Assembly);
        }
    }
}