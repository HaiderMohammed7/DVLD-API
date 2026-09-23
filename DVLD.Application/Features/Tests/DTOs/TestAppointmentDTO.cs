namespace DVLD.Application.Features.Tests.DTOs
{
    public class TestAppointmentDTO
    {
        public decimal PaidFees { get; set; }
        public DateTime AppointmentDate { get; set; }

        public int? RetakeTestApplicationID { get; set; }
        public decimal? RetakeTestPaidFees { get; set; }

        public bool IsLocked { get; set; }
    }
}