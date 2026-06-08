using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepositroy
    {
        private readonly DVLDDbContext _context;

        public CountryRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int CountryId)
        {
            return await _context.Countries.AnyAsync(c => c.CountryID == CountryId);
        }

        public async Task<List<CountryDto>> GetCountryNameAsync()
        {
            return await _context.Countries.OrderBy(c => c.CountryName).Select(c => new CountryDto
            {
                CountryID = c.CountryID,
                CountryName = c.CountryName
            }).ToListAsync();
        }
    }
}