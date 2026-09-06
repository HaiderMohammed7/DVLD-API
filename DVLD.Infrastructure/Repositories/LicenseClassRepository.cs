using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories
{
    public class LicenseClassRepository : ILicenseClassRepository
    {
        private readonly DVLDDbContext _context;

        public LicenseClassRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<List<LicenseClass>> GetAllAsync()
        {
            return await _context.LicenseClass.OrderBy(c => c.ClassName).ToListAsync();
        }

        public async Task<LicenseClass?> GetByIdAsync(int id)
        {
            return await _context.LicenseClass.FirstOrDefaultAsync(x => x.LicenseClassID == id);
        }
    }
}