using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IDetainedLicenseRepository
    {
        Task<DetainedLicense?> GetActiveDetainByLicenseIdAsync(int licenseId);
        Task AddAsync(DetainedLicense detainedLicense);
    }
}