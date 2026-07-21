using DVLD.Application.DTOs;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> EnsureUserExistsAsync(int personId);

        Task<User> GetCurrentUserAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByPersonIdAsync(int personID);
        Task<List<UserListDto>> GetAllAsync();
        Task<UserInfoDto?> GetByIdAsync(int userId);

        Task<int> CreateUserAsync(int newAuthUserId, int personId, UserRole role);
        Task<bool> UpdateAsync(int userId, User request);
        Task<bool> DeleteAsync(int userId);

        Task ActivateUserAsync(int userId);
        Task DeactivateUserAsync(int userId);
        Task AssignRoleAsync(int userId, UserRole role);

        Task<bool> ExistsByIdAsync(int uersId);
        Task<bool> ExistsByPersonIdAsync(int personId);

        Task ChangePasswordAsync(ChangePasswordDto dto);
    }
}