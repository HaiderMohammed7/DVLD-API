using DVLD.Domain.Enums;

namespace DVLD.Application.DTOs
{
    public class CreateUserDto
    {
        public int AuthUserId { get; set; }
        public int PersonId { get; set; }
        public UserRole Role { get; set; }
    }
}