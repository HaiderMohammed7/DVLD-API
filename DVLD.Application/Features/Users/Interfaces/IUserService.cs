using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Users.DTOs;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Features.Users.Interfaces
{
    public interface IUserService
    {
        Task<User> EnsureExistsAsync(int personId);

        Task<CurrentUserDto> GetCurrentUserAsync();
        Task<UserDto?> GetByIdAsync(int userId);
        Task<UserInfoDto?> GetInfoByIdAsync(int userId);
        Task<UserDto?> GetByPersonIdAsync(int personId);
        Task<List<UserListDto>> GetAllAsync();
        

        Task<int> CreateAsync(CreateUserDto dto);
        Task UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int userId);

        Task ActivateAsync(int userId);
        Task DeactivateAsync(int userId);
        Task AssignRoleAsync(int userId, UserRole role);

        Task<bool> ExistsByIdAsync(int uersId);
        Task<bool> ExistsByPersonIdAsync(int personId);

        Task ChangePasswordAsync(ChangePasswordDto dto);
    }
}