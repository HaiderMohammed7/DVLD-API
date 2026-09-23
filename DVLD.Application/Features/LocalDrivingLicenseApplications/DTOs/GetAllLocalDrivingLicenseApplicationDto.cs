namespace DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs
{
    public class GetAllLocalDrivingLicenseApplicationDto
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public string? DrivingClass { get; set; }
        public string? NationalNo { get; set; }
        public string? FullName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int PassedTests { get; set; }
        public string? Status { get; set; }
    }
}