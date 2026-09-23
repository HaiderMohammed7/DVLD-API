using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Users.DTOs;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IAuthApiClient
    {
        Task<List<UserBasicInfoDto>> GetUsersBasicInfoAsync(IEnumerable<int> userIds);
        Task<UserBasicInfoDto?> GetUserByIdAsync(int userId);
        Task ChangePasswordAsync(ChangePasswordDto dto);
        Task<int> RegisterAsync(RegisterUserDto dto);
        Task UpdateUserAsync(int authUserId, UpdateAuthUserDto dto);
        Task DeleteUserAsync(int authUserId);
    }
}