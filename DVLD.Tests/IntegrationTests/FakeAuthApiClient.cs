using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Users.DTOs;
using DVLD.Application.Interfaces.HTTPClient;

namespace DVLD.Tests.IntegrationTests
{
    public class FakeAuthApiClient : IAuthApiClient
    {
        public Task<List<UserBasicInfoDto>> GetBasicInfoAsync(IEnumerable<int> userIds)
        {
            return Task.FromResult(new List<UserBasicInfoDto>());
        }

        public Task<UserBasicInfoDto?> GetByIdAsync(int userId)
        {
            return Task.FromResult<UserBasicInfoDto?>(null);
        }

        public Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            throw new NotSupportedException();
        }

        public Task<int> RegisterAsync(RegisterUserDto dto)
        {
            throw new NotSupportedException();
        }

        public Task UpdateAsync(int authUserId, UpdateAuthUserDto dto)
        {
            throw new NotSupportedException();
        }

        public Task DeleteAsync(int authUserId)
        {
            throw new NotSupportedException();
        }
    }
}