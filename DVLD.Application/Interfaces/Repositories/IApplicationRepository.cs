using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IApplicationRepository
    {
        Task AddAsync(Applications application);
        Task DeleteAsync(int applicationId);
        Task<Applications?> GetByIdAsync(int applicationId);
        Task<bool> UpdateStatusAsync(int applicationId);
    }
}