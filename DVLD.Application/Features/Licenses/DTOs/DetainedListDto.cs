namespace DVLD.Application.DTOs
{
    public class DetainedListDto
    {
        public int DetainId { get; set; }
        public int LicenseId { get; set; }
        public DateTime DetinDate { get; set; }
        public bool IsRelease { get; set; }
        public decimal FineFees { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? NationalNo { get; set; }
        public string? FullName { get; set; }
        public int ReleaseApplicationId { get; set; }
    }
}