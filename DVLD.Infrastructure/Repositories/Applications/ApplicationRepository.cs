using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Enums;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ApplicationEntity = DVLD.Domain.Entities.Applications;

namespace DVLD.Infrastructure.Repositories.Applications
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly DVLDDbContext _context;

        public ApplicationRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ApplicationEntity application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int applicationId)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

            if (application is null)
                return;

            _context.Applications.Remove(application);

            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationEntity?> GetByIdAsync(int applicationId)
        {
            return await _context.Applications.Include(x => x.ApplicationType).Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);
        }

        public async Task<bool> UpdateStatusAsync(int applicationId)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(x => x.ApplicationID == applicationId);
            if (application == null) return false;

            application.ApplicationStatus = ApplicationStatus.Completed;
            application.LastStatusDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}