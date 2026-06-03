using System.Collections;

namespace DVLD.Domain.Entities
{
    public class Person
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string? ImagePath { get; set; }


        public Country Country { get; set; } = null!;
        public User? User { get; set; }
        public ICollection<Driver> Driver { get; set; } = new List<Driver>();
        public ICollection<Applications> Applications { get; set; } = new List<Applications>();
    }
}