using DVLD.Application.DTOs;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DriverEntity = DVLD.Domain.Entities.Driver;

namespace DVLD.Infrastructure.Repositories.Driver
{
    public class DriverRepository : IDriverRepository
    {
        private readonly DVLDDbContext _context;
        public DriverRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<DriverEntity?> GetByPersonIdAsync(int personId)
        {
            return await _context.Drivers.FirstOrDefaultAsync(x => x.PersonID == personId);
        }

        public async Task AddAsync(DriverEntity driver)
        {
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DriverListDto>> GetAllDrivers()
        {
            return await _context.Drivers
               .Select(d => new DriverListDto
               {
                   DriverId = d.DriverID,
                   PersonId = d.PersonID,
                   NationalNo = d.Person!.NationalNo,
                   FullName = d.Person!.FirstName + " " + d.Person.SecondName + " " + d.Person.LastName,
                   CreatedDate = d.CreatedDate,
                   IsActive = d.Licenses.Any(l => l.IsActive),
               }) .AsNoTracking().ToListAsync();
        }
    }
}