using DVLD.Domain.Enums;

namespace DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs
{
    public class ReplaceLicenseDto
    {
        public int LicenseID { get; set; }
        public IssueReasonEnum IssueReason { get; set; }
    }
}