using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories
{
    public class LocalDrivingLicenseApplicationRepository : ILocalDrivingLicenseApplicationRepository
    {
        private readonly DVLDDbContext _context;

        public LocalDrivingLicenseApplicationRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LocalDrivingLicenseApplication application)
        {
            await _context.LocalDrivingLicenseApplications.AddAsync(application);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId)
        {
            return await _context.LocalDrivingLicenseApplications.AnyAsync(x =>
                    x.LicenseClassID == licenseClassId &&
                    x.Applications.ApplicantPersonID == personId &&
                    x.Applications.ApplicationStatus != ApplicationStatus.Cancelled);
        }

        public async Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId,int excludeApplicationId)
        {
            return await _context.LocalDrivingLicenseApplications.AnyAsync(x =>
                x.LocalDrivingLicenseApplicationID != excludeApplicationId &&
                x.LicenseClassID == licenseClassId &&
                x.Applications.ApplicantPersonID == personId &&
                x.Applications.ApplicationStatus != ApplicationStatus.Cancelled);
        }

        public async Task<LocalDrivingLicenseApplication?> GetByIdAsync(int id)
        {
            return await _context.LocalDrivingLicenseApplications.Include(x => x.Applications)
                .FirstOrDefaultAsync(x => x.LocalDrivingLicenseApplicationID == id);
        }
    }
}