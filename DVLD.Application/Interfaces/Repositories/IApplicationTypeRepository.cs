using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IApplicationTypeRepository
    {
        Task<List<ApplicationType>> GetAllAsync();

        Task<ApplicationType?> GetByIdAsync(int id);

        Task UpdateAsync();
    }
}