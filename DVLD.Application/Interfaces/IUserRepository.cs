using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByPersonIdAsync(int personId);
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByAuthUserIdAsync(int authUserId);
        Task<List<User>> GetAllUsers();
        Task<User?> GetUserByIdAsync(int userId);

        Task<User> AddAsync(User user);
        Task UpdateAsync();
        Task DeleteAsync(User user);

        Task<bool> IsUserExist(int userId);
        Task<bool> IsUserExistForPersonId(int personId);
    }
}