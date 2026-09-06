using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;

namespace DVLD.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly DVLDDbContext _context;

        public ApplicationRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Applications application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }
    }
}