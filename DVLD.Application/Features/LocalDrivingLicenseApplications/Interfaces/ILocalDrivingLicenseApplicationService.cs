using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;
namespace DVLD.Application.Features.LocalDrivingLicenseApplications.Interfaces
{
    public interface ILocalDrivingLicenseApplicationService
    {
        Task<int> AddAsync(CreateLocalDrivingLicenseApplicationDto dto);
        Task UpdateAsync(int localDrivingLicenseApplicationId, UpdateLocalDrivingLicenseApplicationDto dto);

        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId);

        Task<GetLocalDrivingLicenseApplicationDto?> GetByIdAsync(int id);
        Task<List<GetAllLocalDrivingLicenseApplicationDto>> GetAllAsync();

        Task CancelAsync(int localDrivingLicenseApplicationId);
        Task DeleteAsync(int id);

        Task<GetLocalDrivingLicenseApplicationInfoDto?> GetForDetailsAsync(int id);

        Task<bool> IsThereAnActiveScheduledTest(int ldlaId, int testTypeID);
        Task<bool> DoesPassTestType(int ldlaId, int testTypeID);
    }
}