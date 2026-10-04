using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.Applications
{
    public class ApplicationTypeRepository : IApplicationTypeRepository
    {
        private readonly DVLDDbContext _context;
        public ApplicationTypeRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationType?> GetByIdAsync(int id)
        {
            return await _context.ApplicationTypes.FirstOrDefaultAsync(x => x.ApplicationTypeID == id);
        }
        public async Task<List<ApplicationType>> GetAllAsync()
        {
            return await _context.ApplicationTypes.AsNoTracking().ToListAsync();
        }  
    }
}