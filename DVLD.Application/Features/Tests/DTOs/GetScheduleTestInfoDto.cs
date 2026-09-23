namespace DVLD.Application.Features.Tests.DTOs
{
    public class GetScheduleTestInfoDto
    {
        public int LocalDrivingLicenseApplicationID { get; set; }

        public string? DrivingClass { get; set; }

        public string? FullName { get; set; }

        public int Trial { get; set; }

        public decimal TestFees { get; set; }

        public decimal RetakeApplicationFees { get; set; }

        public decimal TotalFees { get; set; }

        public int? RetakeTestApplicationID { get; set; }
    }
}