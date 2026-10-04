using DVLD.Application.DTOs;
using DVLD.Application.Features.Licenses.DTOs;
using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;

namespace DVLD.Application.Features.Licenses.Interfaces
{
    public interface ILicenseService
    {
        Task<GetLicenseInfoDto?> GetByIdAsync(int licenseID);
        Task<int> IssueAsync(IssueDriverLicenseDto dto);
        Task<int> DetainAsync(DetainLicenseDto dto);
        Task<int> ReleaseAsync(int licenseID);
        Task<ResponseRenewLicenseDto?> RenewAsync(int licenseId, string? notes);
        Task<ResponseReplaceLicenseDto?> ReplaceAsync(ReplaceLicenseDto dto);
        Task<GetReleaseLicenseInfo?> ReleaseInfo(int licenseId);
        Task<List<DetainedListDto>> GetDetainedList();
    }
}