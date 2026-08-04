using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories
{
    public class ApplicationTypeRepository : IApplicationTypeRepository
    {
        private readonly DVLDDbContext _context;
        public ApplicationTypeRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationType>> GetAllAsync()
        {
            return await _context.ApplicationTypes.AsNoTracking().ToListAsync();
        }

        public async Task<ApplicationType?> GetByIdAsync(int id)
        {
            return await _context.ApplicationTypes.FirstOrDefaultAsync(x => x.ApplicationTypeID == id);
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}