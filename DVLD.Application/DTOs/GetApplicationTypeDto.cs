namespace DVLD.Application.DTOs
{
    public class GetApplicationTypeDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Fees { get; set; }
    }
}