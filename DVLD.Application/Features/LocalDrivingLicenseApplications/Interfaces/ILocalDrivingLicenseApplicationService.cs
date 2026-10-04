using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;
namespace DVLD.Application.Features.LocalDrivingLicenseApplications.Interfaces
{
    public interface ILocalDrivingLicenseApplicationService
    {
        Task<GetLocalDrivingLicenseApplicationDto?> GetByIdAsync(int id);
        Task<GetLocalDrivingLicenseApplicationInfoDto?> GetInfoByIdAsync(int id);
        Task<List<GetAllLocalDrivingLicenseApplicationDto>> GetAllAsync();

        Task<int> AddAsync(CreateLocalDrivingLicenseApplicationDto dto);
        Task UpdateAsync(int localDrivingLicenseApplicationId, UpdateLocalDrivingLicenseApplicationDto dto);
        Task CancelAsync(int localDrivingLicenseApplicationId);
        Task DeleteAsync(int id);

        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId);
        Task<bool> IsThereAnActiveScheduledTest(int ldlaId, int testTypeID);
        Task<bool> DoesPassTestType(int ldlaId, int testTypeID);
    }
}