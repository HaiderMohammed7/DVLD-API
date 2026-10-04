using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.Tests
{
    public class TestRepository : ITestRepository
    {
        private readonly DVLDDbContext _context;

        public TestRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<Test?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Tests.FirstOrDefaultAsync(x => x.TestAppointmentID == appointmentId);
        }
        public async Task<Test?> GetByIdAsync(int testId)
        {
            return await _context.Tests.FirstOrDefaultAsync(x => x.TestID == testId);
        }

        public async Task AddAsync(Test test)
        {
            await _context.Tests.AddAsync(test);
        }     
    }
}