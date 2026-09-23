using DVLD.Application.Features.Tests.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ILocalDrivingLicenseApplicationRepository
    {
        Task AddAsync(LocalDrivingLicenseApplication application);
        Task UpdateAsync();
        Task<bool> DeleteAsync(int id);

        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId);
        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId, int excludeApplicationId);

        Task<LocalDrivingLicenseApplication?> GetByIdAsync(int id);
        Task<List<LocalDrivingLicenseApplication>> GetAllAsync();

        Task<LocalDrivingLicenseApplication?> GetForDetailsAsync(int id);

        Task<bool> DoesPassTestType(int localDrivingLicenseApplicationID, int testTypeID);
        Task<GetScheduleTestInfoDto?> GetScheduleTestInfoAsync(int localDrivingLicenseApplicationID, int testTypeID);
    }
}