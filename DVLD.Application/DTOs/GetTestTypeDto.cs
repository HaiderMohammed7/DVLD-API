namespace DVLD.Application.DTOs
{
    public class GetTestTypeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Fees { get; set; }
    }
}