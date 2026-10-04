using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IApplicationTypeRepository
    {
        Task<ApplicationType?> GetByIdAsync(int id);
        Task<List<ApplicationType>> GetAllAsync();      
    }
}