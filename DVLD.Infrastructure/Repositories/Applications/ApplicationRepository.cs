using DVLD.Application.Interfaces.Repositories;
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

        public async Task<ApplicationEntity?> GetByIdAsync(int applicationId)
        {
            return await _context.Applications.Include(x => x.ApplicationType).Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);
        }

        public async Task AddAsync(ApplicationEntity application)
        {
            await _context.Applications.AddAsync(application);
        }
        public async Task DeleteAsync(ApplicationEntity application)
        {
            _context.Applications.Remove(application);
        }
    }
}