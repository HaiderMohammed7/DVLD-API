namespace DVLD.Domain.Entities
{
    public class TestAppointment
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }

        public TestType? TestType { get; set; }
        public Applications? Applications { get; set; }
        public User? User { get; set; }
        public LocalDrivingLicenseApplication? localDrivingLicenseApplication { get; set; }
        public ICollection<Test> Tests { get; set; } = new List<Test>();
    }
}