namespace DVLD.Application.DTOs
{
    public class GetDriverLocalLicenseDto
    {
        public int LicenseId { get; set; }
        public int ApplicationId { get; set; }
        public string? ClassName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool ISActive { get; set; }
    }
}