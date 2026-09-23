using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ILicenseClassRepository
    {
        Task<List<LicenseClass>> GetAllAsync();
        Task<LicenseClass?> GetByIdAsync(int id);
    }
}