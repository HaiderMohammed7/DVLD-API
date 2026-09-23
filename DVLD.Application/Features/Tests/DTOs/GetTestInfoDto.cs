namespace DVLD.Application.Features.Tests.DTOs
{
    public class GetTestInfoDto
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string? Notes { get; set; }
        public int CreatedByUserID { get; set; }
    }
}