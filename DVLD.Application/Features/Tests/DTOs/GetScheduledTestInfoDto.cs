namespace DVLD.Application.Features.Tests.DTOs
{
    public class GetScheduledTestInfoDto
    {
        public int LocalDrivingLicenseApplicationID { get; set; }

        public string? DrivingClass { get; set; }

        public string? FullName { get; set; }

        public int Trial { get; set; }

        public DateTime AppointmentDate { get; set; }

        public decimal PaidFees { get; set; }

        public int? TestID { get; set; }
    }
}