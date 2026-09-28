using DVLD.Application.Features.Licenses.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IDetainedLicenseRepository
    {
        Task<DetainedLicense?> GetActiveDetainByLicenseIdAsync(int licenseId);
        Task AddAsync(DetainedLicense detainedLicense);
        Task UpdateForReleaseAsync(UpdateDetainLicenseDto dto);
        Task<List<DetainedLicense>> GetDetainedList();
    }
}