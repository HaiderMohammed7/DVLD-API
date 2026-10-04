using DVLD.Application.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ILicenseRepository
    {
        Task<License?> GetByIdAsync(int licenseID);
        Task<License?> GetActiveLicenseByDriverIdAndClassAsync(int driverId, int licenseClassId);
        Task AddAsync(License license);

        Task<List<GetDriverLocalLicenseDto>> GetDriverLocalLicensesAsync(int personId);
        Task<License?> GetValidLicenseForInternationalAsync(int licenseId);
    }
}