using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByPersonIdAsync(int personId);
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByAuthUserIdAsync(int authUserId);
        Task<List<User>> GetAllAsync();

        Task AddAsync(User user);
        Task DeleteAsync(User user);

        Task<bool> IsExist(int userId);
        Task<bool> IsExistByPersonId(int personId);
    }
}