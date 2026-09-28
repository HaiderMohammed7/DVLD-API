using DVLD.Application.DTOs;
using DVLD.Application.Features.Licenses.DTOs;

namespace DVLD.Application.Features.Licenses.Interfaces
{
    public interface ILicenseService
    {
        Task<GetLicenseInfoDto?> GetForDetailsAsync(int licenseID);
        Task<int> IssueDriverLicenseAsync(IssueDriverLicenseDto dto);
        Task<int> DetainLicenseAsync(DetainLicenseDto dto);
        Task<int> ReleaseDetainedLicenseAsync(int licenseID);
        Task<GetReleaseLicenseInfo?> ReleaseInfo(int licenseId);
        Task<List<DetainedListDto>> GetDetainedList();
    }
}