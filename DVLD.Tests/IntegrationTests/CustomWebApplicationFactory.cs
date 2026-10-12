using DVLD.Application.Interfaces.HTTPClient;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using DVLD.Infrastructure.Data;
using DVLD.Tests.IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const int TestAuthUserId = 999999;
    private const string TestJwtSecret = "this-is-a-test-secret-key-for-integration-tests-123456";

    public int TestLocalDrivingLicenseApplicationId { get; private set; }
    public int TestVisionTestTypeId { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var testSettings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=.;Database=DVLD_Test;Trusted_Connection=True;TrustServerCertificate=True;"
            };

            config.AddInMemoryCollection(testSettings);
        });

        Environment.SetEnvironmentVariable("JWT_SECRET", TestJwtSecret);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IAuthApiClient>();

            services.AddScoped<IAuthApiClient, FakeAuthApiClient>();

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<DVLDDbContext>();

            db.Database.Migrate();

            SeedTestData(db);
        });
    }

    private void SeedTestData(DVLDDbContext db)
    {
        var country = db.Countries.FirstOrDefault(x => x.CountryName == "Test Country");

        if (country == null)
        {
            country = new Country{ CountryName = "Test Country"};

            db.Countries.Add(country);
            db.SaveChanges();
        }

        var person = db.People.FirstOrDefault(x => x.NationalNo == "TEST-ADMIN-001");

        if (person == null)
        {
            person = new Person
            {
                NationalNo = "TEST-ADMIN-001",
                FirstName = "Test",
                SecondName = "Admin",
                ThirdName = null,
                LastName = "User",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gendor = 1,
                Address = "Test Address",
                Phone = "0000000000",
                Email = "testadmin@test.com",
                NationalityCountryID = country.CountryID,
                ImagePath = null
            };

            db.People.Add(person);
            db.SaveChanges();
        }

        var user = db.Users.FirstOrDefault(x => x.AuthUserId == TestAuthUserId);

        if (user == null)
        {
            user = new User
            {
                AuthUserId = TestAuthUserId,
                PersonID = person.PersonID,
                Role = UserRole.Admin,
                IsActive = true
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        var applicationType = db.ApplicationTypes.FirstOrDefault(x => x.ApplicationTypeTitle == "Test Application");

        if (applicationType == null)
        {
            applicationType = new ApplicationType
            {
                ApplicationTypeTitle = "Test Application",
                ApplicationFees = 5
            };

            db.ApplicationTypes.Add(applicationType);
            db.SaveChanges();
        }

        var licenseClass = db.LicenseClass.FirstOrDefault(x => x.ClassName == "Test Class");

        if (licenseClass == null)
        {
            licenseClass = new LicenseClass
            {
                ClassName = "Test Class",
                ClassDescription = "Test License Class",
                MinimumAllowedAge = 18,
                DefaultValidityLength = 10,
                ClassFees = 50
            };

            db.LicenseClass.Add(licenseClass);
            db.SaveChanges();
        }

        var application = db.Applications.FirstOrDefault(x => x.ApplicantPersonID == person.PersonID &&x.ApplicationTypeID == applicationType.ApplicationTypeID);

        if (application == null)
        {
            application = new Applications
            {
                ApplicantPersonID = person.PersonID,
                ApplicationTypeID = applicationType.ApplicationTypeID,
                ApplicationDate = DateTime.UtcNow,
                ApplicationStatus = ApplicationStatus.New,
                LastStatusDate = DateTime.UtcNow,
                PaidFees = applicationType.ApplicationFees,
                CreatedByUserID = user.UserID
            };

            db.Applications.Add(application);
            db.SaveChanges();
        }

        var ldla = db.LocalDrivingLicenseApplications.FirstOrDefault(x => x.ApplicationID == application.ApplicationID);

        if (ldla == null)
        {
            ldla = new LocalDrivingLicenseApplication
            {
                ApplicationID = application.ApplicationID,
                LicenseClassID = licenseClass.LicenseClassID
            };

            db.LocalDrivingLicenseApplications.Add(ldla);
            db.SaveChanges();
        }

        var testType = db.TestTypes.FirstOrDefault(x => x.TestTypeTitle == "Vision Test");

        if (testType == null)
        {
            testType = new TestType
            {
                TestTypeTitle = "Vision Test",
                TestTypeDescription = "Test Vision",
                TestTypeFees = 10
            };

            db.TestTypes.Add(testType);
            db.SaveChanges();
        }

        TestLocalDrivingLicenseApplicationId = ldla.LocalDrivingLicenseApplicationID;
        TestVisionTestTypeId = testType.TestTypeID;
    }
}