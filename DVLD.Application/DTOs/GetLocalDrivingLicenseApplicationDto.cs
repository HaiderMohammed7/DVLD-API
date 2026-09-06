namespace DVLD.Application.DTOs
{
    public class GetLocalDrivingLicenseApplicationDto
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int PersonId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int LicenseClassId { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
    }
}