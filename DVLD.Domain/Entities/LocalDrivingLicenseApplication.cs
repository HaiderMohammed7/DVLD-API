namespace DVLD.Domain.Entities
{
    public class LocalDrivingLicenseApplication
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public Applications? Applications { get; set; }
        public LicenseClass? LicenseClass { get; set; }
        public ICollection<TestAppointment> TestAppointments { get; set; } = new List<TestAppointment>();
    }
}