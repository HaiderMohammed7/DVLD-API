using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.Tests
{
    public class TestTypeRepository : ITestTypeRepository
    {
        private readonly DVLDDbContext _context;

        public TestTypeRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<List<TestType>> GetAllAsync()
        {
            return await _context.TestTypes.AsNoTracking().ToListAsync();
        }
        public async Task<TestType?> GetByIdAsync(int id)
        {
            return await _context.TestTypes.FirstOrDefaultAsync(x => x.TestTypeID == id);
        }
    }
}