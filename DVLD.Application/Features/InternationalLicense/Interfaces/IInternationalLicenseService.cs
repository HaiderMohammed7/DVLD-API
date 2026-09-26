using DVLD.Application.DTOs;

namespace DVLD.Application.Features.InternationalLicense.Interfaces
{
    public interface IInternationalLicenseService
    {
        Task<GetInternationalLicenseInfoDto?> GetInternationalLicenseInfoAsync(int internationalLicenseId);
        Task<int> IssueInternationalLicenseAsync(int licenseId);
        Task<List<ListInternationalLicenseApplicationDto>> GetAllAsync();
    }
}