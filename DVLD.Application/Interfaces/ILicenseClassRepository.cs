using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface ILicenseClassRepository
    {
        Task<List<LicenseClass>> GetAllAsync();
        Task<LicenseClass?> GetByIdAsync(int id);
    }
}