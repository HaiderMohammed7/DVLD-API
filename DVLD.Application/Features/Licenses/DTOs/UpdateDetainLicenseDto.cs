namespace DVLD.Application.Features.Licenses.DTOs
{
    public class UpdateDetainLicenseDto
    {
        public int DetainId { get; set; }
        public int ReleasedByUserId { get; set; }
        public int ReleaseApplicationId { get; set; }
        public DateTime releaseDate { get; set; }
    }
}