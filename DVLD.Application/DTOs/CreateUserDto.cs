namespace DVLD.Application.DTOs
{
    public class CreateUserDto
    {
        public int PersonId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}