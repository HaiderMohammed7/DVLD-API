namespace DVLD.Application.DTOs
{
    public class DriverListDto
    {
        public int DriverId { get; set; }
        public int PersonId { get; set; }
        public string? NationalNo { get; set; }
        public string? FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}