namespace DVLD.Domain.Entities
{
    public class TestType
    {
        public int TestTypeID { get; set; }

        public string TestTypeTitle { get; set; } = null!;

        public string TestTypeDescription { get; set; } = null!;

        public decimal TestTypeFees { get; set; }
    }
}