using DVLD.Application.Features.Licenses.DTOs;
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
            return await _context.DetainedLicense.Include(x => x.License).ThenInclude(x => x.Driver).FirstOrDefaultAsync(x => x.LicenseID == licenseId &&!x.IsReleased);
        }

        public async Task AddAsync(DetinedEntity detainedLicense)
        {
            await _context.DetainedLicense.AddAsync(detainedLicense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateForReleaseAsync(UpdateDetainLicenseDto dto)
        {
            var detainedLicense = await _context.DetainedLicense .FirstOrDefaultAsync(x => x.DetainID == dto.DetainId);

            if (detainedLicense == null) return;

            detainedLicense.IsReleased = true;
            detainedLicense.ReleaseDate = dto.releaseDate;
            detainedLicense.ReleasedByUserID = dto.ReleasedByUserId;
            detainedLicense.ReleaseApplicationID = dto.ReleaseApplicationId;

            await _context.SaveChangesAsync();
        }

        public async Task<List<DetinedEntity>> GetDetainedList()
        {
            return await _context.DetainedLicense.Include(x => x.License).ThenInclude(x => x.Driver).
               ThenInclude(x => x.Person).AsNoTracking().ToListAsync();
        }
    }
}