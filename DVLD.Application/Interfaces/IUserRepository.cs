using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByPersonIdAsync(int personId);
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByAuthUserIdAsync(int authUserId);

        Task AddAsync(User user);

        Task UpdateAsync();
    }
}