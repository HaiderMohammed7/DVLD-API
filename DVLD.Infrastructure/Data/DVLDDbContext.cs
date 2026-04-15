using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Data
{
    public class DVLDDbContext : DbContext
    {
        public DVLDDbContext(DbContextOptions<DVLDDbContext> options) : base(options)
        {
        }

        public DbSet<Applications> Application { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<DetainedLicense> DetainedLicense { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<InternationalLicense> InternationalLicense { get; set; }
        public DbSet<LicenseClass> LicenseClass { get; set; }
        public DbSet<License> License { get; set; }
        public DbSet<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<TestAppointment> TestAppointment { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<TestType> TestType { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DVLDDbContext).Assembly);
        }
    }
}