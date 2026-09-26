using DVLD.Application.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IInternationalLicenseRepository
    {
        Task<GetInternationalLicenseInfoDto?> GetInternationalLicenseInfoAsync(int internationalLicenseId);
        Task<List<GetDriverInternationalLicenseDto>> GetDriverInternationalLicensesAsync(int personId);
        Task<InternationalLicense?> GetActiveByDriverIdAsync(int driverId);
        Task AddAsync(InternationalLicense entity);
        Task<List<InternationalLicense>> GetAllAsync();
    }
}