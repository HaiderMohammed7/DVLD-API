using DVLD.Application.Features.Tests.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ILocalDrivingLicenseApplicationRepository
    {
        Task AddAsync(LocalDrivingLicenseApplication ldla);
        Task DeleteAsync(LocalDrivingLicenseApplication ldla);

        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId);
        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId, int excludeApplicationId);

        Task<LocalDrivingLicenseApplication?> GetByIdAsync(int id);
        Task<List<LocalDrivingLicenseApplication>> GetAllAsync();

        Task<bool> DoesPassTestType(int localDrivingLicenseApplicationID, int testTypeID);
        Task<GetScheduleTestInfoDto?> GetScheduleTestInfoAsync(int localDrivingLicenseApplicationID, int testTypeID);
    }
}