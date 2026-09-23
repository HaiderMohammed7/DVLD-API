using DVLD.Application.DTOs;

namespace DVLD.Application.Features.InternationalLicense.Interfaces
{
    public interface IInternationalLicenseService
    {
        Task<GetInternationalLicenseInfoDto?> GetInternationalLicenseInfoAsync(int internationalLicenseId);
    }
}