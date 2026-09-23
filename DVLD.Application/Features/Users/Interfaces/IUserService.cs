using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Users.DTOs;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Features.Users.Interfaces
{
    public interface IUserService
    {
        Task<User> EnsureUserExistsAsync(int personId);

        Task<User> GetCurrentUserAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByPersonIdAsync(int personID);
        Task<List<UserListDto>> GetAllAsync();
        Task<UserInfoDto?> GetByIdAsync(int userId);

        Task<int> CreateUserAsync(CreateUserDto dto);
        Task UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int userId);

        Task ActivateUserAsync(int userId);
        Task DeactivateUserAsync(int userId);
        Task AssignRoleAsync(int userId, UserRole role);

        Task<bool> ExistsByIdAsync(int uersId);
        Task<bool> ExistsByPersonIdAsync(int personId);

        Task ChangePasswordAsync(ChangePasswordDto dto);
    }
}