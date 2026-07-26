using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace DVLD.Infrastructure.HTTPClient
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserBasicInfoDto>> GetUsersBasicInfoAsync(IEnumerable<int> userIds)
        {
            var request = new GetUsersBasicInfoRequest
            {
                UserIds = userIds.ToList()
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/basic-info", request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<UserBasicInfoDto>>();

            return result ?? new List<UserBasicInfoDto>();
        }
        public async Task<UserBasicInfoDto?> GetUserByIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/auth/{userId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserBasicInfoDto>();
        }
        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/change-password", dto);

            response.EnsureSuccessStatusCode();
        }
        public async Task<int> RegisterAsync(RegisterUserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto<int>>();

            return result!.Data;
        }
    }
}