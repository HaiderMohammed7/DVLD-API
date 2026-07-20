namespace DVLD.Application.DTOs
{
    public class GetUsersBasicInfoRequest
    {
        public List<int> UserIds { get; set; } = new();
    }
}