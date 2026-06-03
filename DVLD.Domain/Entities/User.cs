using DVLD.Domain.Enums;

namespace DVLD.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public int AuthUserId { get; set; }
        public int PersonID { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }

        public Person Person { get; set; } = null!;

        public ICollection<Applications> CreatedApplications { get; set; } = new List<Applications>();
        public ICollection<Driver> CreatedDrivers { get; set; } = new List<Driver>();
        public ICollection<TestAppointment> CreatedAppointments { get; set; } = new List<TestAppointment>();
        public ICollection<Test> CreatedTests { get; set; } = new List<Test>();
        public ICollection<License> CreatedLicenses { get; set; } = new List<License>();
        public ICollection<InternationalLicense> CreatedInternationalLicenses { get; set; } = new List<InternationalLicense>();
        public ICollection<DetainedLicense> CreatedDetainedLicenses { get; set; } = new List<DetainedLicense>();
    }
}