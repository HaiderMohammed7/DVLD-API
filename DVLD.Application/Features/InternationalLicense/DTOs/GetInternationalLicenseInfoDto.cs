namespace DVLD.Application.DTOs
{
    public class GetInternationalLicenseInfoDto
    {
        public int InternationalLicenseId { get; set; }
        public int ApplicationId { get; set; }
        public int LocalLicenseId { get; set; }
        public int DriverId { get; set; }
        public string? FullName { get; set; }
        public string? NationalNo { get; set; }
        public string? ImagePath { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public byte Gendor { get; set; }
    }
}