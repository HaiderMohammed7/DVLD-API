namespace DVLD.Domain.Entities
{
    public class InternationalLicense
    {
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public User? User { get; set; }
        public Driver? Driver { get; set; }
        public License? License { get; set; }
        public Applications? Applications { get; set; }
    }
}