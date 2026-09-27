using DVLD.Application.DTOs;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.Licenses
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly DVLDDbContext _context;

        public LicenseRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<License?> GetForDetailsAsync(int licenseID)
        {
            return await _context.Licenses.Include(x => x.Classes).Include(x => x.Driver)
                    .ThenInclude(x => x.Person).Include(x => x.DetainedLicenses).FirstOrDefaultAsync(x => x.LicenseID == licenseID);
        }

        public async Task<License?> GetActiveLicenseByDriverIdAndClassAsync(int driverId, int licenseClassId)
        {
            return await _context.Licenses.FirstOrDefaultAsync(x => x.DriverID == driverId && x.LicenseClass == licenseClassId && x.IsActive);
        }

        public async Task AddAsync(License license)
        {
            await _context.Licenses.AddAsync(license);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetDriverLocalLicenseDto>> GetDriverLocalLicensesAsync(int personId)
        {
            return await _context.Licenses.Where(l => l.Driver!.PersonID == personId).OrderByDescending(l => l.IsActive)
                .ThenByDescending(l => l.ExpirationDate)
                .Select(l => new GetDriverLocalLicenseDto
                {
                    LicenseId = l.LicenseID,
                    ApplicationId = l.ApplicationID,
                    ClassName = l.Classes!.ClassName,
                    IssueDate = l.IssueDate,
                    ExpirationDate = l.ExpirationDate,
                    ISActive = l.IsActive
                }).ToListAsync();
        }

        public async Task<License?> GetValidLicenseForInternationalAsync(int licenseId)
        {
            return await _context.Licenses.Include(x => x.Driver).FirstOrDefaultAsync(x => x.LicenseID == licenseId &&
                    x.LicenseClass == 3 && x.IsActive && x.ExpirationDate > DateTime.Now);
        }

        public async Task<bool> DeactivateAsync(int licenseId)
        {
            var license = await _context.Licenses.FirstOrDefaultAsync(x => x.LicenseID == licenseId);
            if (license == null) return false;

            license.IsActive = false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}