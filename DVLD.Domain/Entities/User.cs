namespace DVLD.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }

        public int PersonID { get; set; }

        public string UserName { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}