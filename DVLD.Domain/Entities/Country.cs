namespace DVLD.Domain.Entities
{
    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; } = null!;

        public ICollection<Person> People { get; set; } = new List<Person>();
    }
}