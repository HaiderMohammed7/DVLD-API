namespace DVLD.Application.Features.Applications.DTOs
{
    public class GetApplicationInfoDto
    {
        public int ApplicationID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = null!;
        public DateTime StatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public string ApplicationType { get; set; } = null!;
        public int ApplicantPersonID { get; set; }
        public string ApplicantName { get; set; } = null!;
        public int CreatedByUserID { get; set; }
    }
}