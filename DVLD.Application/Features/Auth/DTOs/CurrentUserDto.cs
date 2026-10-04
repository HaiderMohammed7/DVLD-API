namespace DVLD.Application.Features.Auth.DTOs
{
    public class CurrentUserDto
    {
        public int UserID { get; set; }
        public int AuthUserId { get; set; }
        public int PersonID { get; set; }
        public string? Role { get; set; }
    }
}