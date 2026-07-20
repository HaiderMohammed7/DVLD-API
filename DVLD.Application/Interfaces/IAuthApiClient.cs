using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface IAuthApiClient
    {
        Task<List<UserBasicInfoDto>> GetUsersBasicInfoAsync(IEnumerable<int> userIds);
        Task<UserBasicInfoDto?> GetUserByIdAsync(int userId);
    }
}