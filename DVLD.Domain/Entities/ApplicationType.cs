namespace DVLD.Domain.Entities
{
    public class ApplicationType
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; } = null!;
        public decimal ApplicationFees { get; set; }

        public ICollection<Applications> Applications { get; set; } = new List<Applications>();
    }
}