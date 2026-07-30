namespace DVLD.Application.DTOs
{
    public class UpdateUserDto
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}