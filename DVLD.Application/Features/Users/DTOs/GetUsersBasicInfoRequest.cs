namespace DVLD.Application.Features.Users.DTOs
{
    public class GetUsersBasicInfoRequest
    {
        public List<int> UserIds { get; set; } = new();
    }
}