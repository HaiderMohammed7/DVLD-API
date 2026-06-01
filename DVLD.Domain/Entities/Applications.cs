namespace DVLD.Domain.Entities
{
    public class Applications
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }

        public User? User { get; set; }
        public Person? Person { get; set; }
        public ApplicationType? ApplicationType { get; set; }
        public ICollection<License> Licenses { get; set; } = new List<License>();
        public ICollection<DetainedLicense> DetainedLicenses { get; set; } = new List<DetainedLicense>();
        public ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();
        public ICollection<TestAppointment> TestAppointments { get; set; } = new List<TestAppointment>();
        public ICollection<LocalDrivingLicenseApplication> localDrivingLicenseApplications { get; set; } = new List<LocalDrivingLicenseApplication>();
    }
}