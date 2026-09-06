using DVLD.Application.DTOs;
namespace DVLD.Application.Interfaces
{
    public interface ILocalDrivingLicenseApplicationService
    {
        Task<int> AddAsync(CreateLocalDrivingLicenseApplicationDto dto);
        Task UpdateAsync(int localDrivingLicenseApplicationId, UpdateLocalDrivingLicenseApplicationDto dto);

        Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId);
        Task<GetLocalDrivingLicenseApplicationDto?> GetByIdAsync(int id);
    }
}