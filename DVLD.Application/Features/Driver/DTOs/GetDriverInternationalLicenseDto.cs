namespace DVLD.Application.DTOs
{
    public class GetDriverInternationalLicenseDto
    {
        public int InternationalLicenseId { get; set; }
        public int ApplicationId { get; set; }
        public int LocalLicenseId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}