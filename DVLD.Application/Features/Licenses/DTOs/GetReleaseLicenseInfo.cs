namespace DVLD.Application.DTOs
{
    public class GetReleaseLicenseInfo
    {
        public int DetainId { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal ApplicationFees { get; set; }
        public decimal FineFees { get; set; }
    }
}