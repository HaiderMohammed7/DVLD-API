using DVLD.Domain.Contracts;

namespace DVLD.Application.Features.Users.DTOs
{
    public class UserDto : IOwnable<int>
    {
        public int UserId { get; set; }
        public int AuthUserId { get; set; }
        public int PersonId { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }

        public int OwnerId => PersonId;
    }
}