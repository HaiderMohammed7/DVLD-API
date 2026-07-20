namespace DVLD.Application.DTOs
{
    public class UserListDto
    {
        public int UserId { get; set; }
        public int PersonId { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public bool IsActive { get; set; }
    }
}