namespace DVLD.Application.Features.Users.DTOs
{
    public class UserInfoDto
    {
        public int UserId { get; set; }

        public int PersonId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}