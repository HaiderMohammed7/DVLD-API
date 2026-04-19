using DVLD.Domain.Enums;

namespace DVLD.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public int AuthUserId { get; set; }

        public int PersonID { get; set; }

        public UserRole Role { get; set; }

        public bool IsActive { get; set; }
    }
}