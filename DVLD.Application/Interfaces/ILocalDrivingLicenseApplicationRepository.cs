using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface ILocalDrivingLicenseApplicationRepository
    {
        Task AddAsync(LocalDrivingLicenseApplication application);
        Task UpdateAsync();

        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId);
        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId, int excludeApplicationId);

        Task<LocalDrivingLicenseApplication?> GetByIdAsync(int id);
    }
}