using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IInternationalLicenseRepository
    {
        Task<GetInternationalLicenseInfoDto?> GetInternationalLicenseInfoAsync(int internationalLicenseId);
        Task<List<GetDriverInternationalLicenseDto>> GetDriverInternationalLicensesAsync(int personId);
    }
}