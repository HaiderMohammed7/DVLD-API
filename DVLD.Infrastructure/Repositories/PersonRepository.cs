using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DVLDDbContext _context;

        public PersonRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<Person?> GetByAuthUserIdAsync(int authUserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AuthUserId == authUserId);

            if (user == null)
                return null;

            return await _context.People.FirstOrDefaultAsync(p => p.PersonID == user.PersonID);
        }
        public async Task<Person?> GetByIdAsync(int personId)
        {
            return await _context.People.FindAsync(personId);
        }
        public async Task<Person?> GetByNationalNoAsync(string nationalNo)
        {
            return await _context.People.FirstOrDefaultAsync(p => p.NationalNo == nationalNo);
        }
        public async Task<List<PeopleListDto>> GetAllAsync()
        {
            return await _context.People.AsNoTracking()
                .Select(p => new PeopleListDto
                    {
                        PersonID = p.PersonID,
                        NationalNo = p.NationalNo,
                        FirstName = p.FirstName,
                        SecondName = p.SecondName,
                        ThirdName = p.ThirdName,
                        LastName = p.LastName,

                        DateOfBirth = p.DateOfBirth,

                        Gender = p.Gendor == 0
                            ? "Female"
                            : "Male",

                        Phone = p.Phone,
                        Email = p.Email,
                        
                        Nationality = p.Country.CountryName

                    }).ToListAsync();
        }

        public async Task<Person> AddAsync(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            return person;
        }
        public async Task<bool> UpdateAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int personId)
        {
            var person = await _context.People.FindAsync(personId);

            if (person == null) return false;

            _context.People.Remove(person);       

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByNationalNoAsync(string nationalNo)
        {
            return await _context.People.AnyAsync(p => p.NationalNo == nationalNo);
        }
        public async Task<bool> ExistsByIdAsync(int personId)
        {
            return await _context.People.AnyAsync(p => p.PersonID == personId);
        }

        public async Task<string?> GetCountryNameByIdAsync(int personId)
        {
            return await _context.People.Where(p => p.PersonID == personId).Select(p => p.Country.CountryName).FirstOrDefaultAsync();
        }
    }
}