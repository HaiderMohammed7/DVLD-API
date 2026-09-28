using DVLD.Application.Interfaces.Repositories;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DetinedEntity = DVLD.Domain.Entities.DetainedLicense;

namespace DVLD.Infrastructure.Repositories.DetainedLicense
{
    public class DetainedLicenseRepository : IDetainedLicenseRepository
    {
        private readonly DVLDDbContext _context;

        public DetainedLicenseRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<DetinedEntity?> GetActiveDetainByLicenseIdAsync(int licenseId)
        {
            return await _context.DetainedLicense.FirstOrDefaultAsync(x => x.LicenseID == licenseId && !x.IsReleased);
        }

        public async Task AddAsync(DetinedEntity detainedLicense)
        {
            await _context.DetainedLicense.AddAsync(detainedLicense);
            await _context.SaveChangesAsync();
        }
    }
}