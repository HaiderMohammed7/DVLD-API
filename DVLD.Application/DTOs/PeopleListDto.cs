namespace DVLD.Application.DTOs
{
    public class PeopleListDto
    {
        public int PersonID { get; set; }

        public string NationalNo { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string SecondName { get; set; } = null!;

        public string? ThirdName { get; set; }

        public string LastName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }
    }
}