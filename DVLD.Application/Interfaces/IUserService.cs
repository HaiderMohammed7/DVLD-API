using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> EnsureUserExistsAsync(int personId);

        Task<int> CreateUserAsync(int newAuthUserId, int personId, UserRole role);

        Task ActivateUserAsync(int userId);

        Task DeactivateUserAsync(int userId);

        Task AssignRoleAsync(int userId, UserRole role);
    }
}