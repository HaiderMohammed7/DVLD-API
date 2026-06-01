namespace DVLD.Domain.Entities
{
    public class Driver
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public User? User { get; set; }
        public Person? Person { get; set; }

        public ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();
        public ICollection<License> Licenses { get; set; } = new List<License>();
    }
}