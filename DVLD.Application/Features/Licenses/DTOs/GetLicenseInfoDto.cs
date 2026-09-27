namespace DVLD.Application.Features.Licenses.DTOs
{
    public class GetLicenseInfoDto
    {
        public string LicenseClassName { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string NationalNo { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;

        public int DriverID { get; set; }

        public byte DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        public bool IsActive { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string IssueReason { get; set; } = null!;

        public bool IsDetained { get; set; }

        public string? Notes { get; set; }

        public string? ImagePath { get; set; }
    }
}