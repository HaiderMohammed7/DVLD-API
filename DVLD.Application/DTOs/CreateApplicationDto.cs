namespace DVLD.Application.DTOs
{
    public class CreateApplicationDto
    {
        public int ApplicantPersonId { get; set; }

        public int ApplicationTypeId { get; set; }

        public decimal PaidFees { get; set; }
    }
}