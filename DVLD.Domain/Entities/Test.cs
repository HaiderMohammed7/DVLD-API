namespace DVLD.Domain.Entities
{
    public class Test
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string? Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public User? User { get; set; }
        public TestAppointment? TestAppointment { get; set; }
    }
}