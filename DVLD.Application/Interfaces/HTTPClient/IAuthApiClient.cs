using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Users.DTOs;

namespace DVLD.Application.Interfaces.HTTPClient
{
    public interface IAuthApiClient
    {
        Task<List<UserBasicInfoDto>> GetBasicInfoAsync(IEnumerable<int> userIds);
        Task<UserBasicInfoDto?> GetByIdAsync(int userId);
        Task ChangePasswordAsync(ChangePasswordDto dto);
        Task<int> RegisterAsync(RegisterUserDto dto);
        Task UpdateAsync(int authUserId, UpdateAuthUserDto dto);
        Task DeleteAsync(int authUserId);
    }
}