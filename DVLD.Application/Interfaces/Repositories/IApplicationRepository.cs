using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IApplicationRepository
    {
        Task<Applications?> GetByIdAsync(int applicationId);

        Task AddAsync(Applications application);
        Task DeleteAsync(Applications application);       
    }
}