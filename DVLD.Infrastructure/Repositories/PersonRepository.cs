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
        public async Task<List<Person>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> AddAsync(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            return person;
        }
        public async Task<bool> UpdateAsync(Person person)
        {
            var existingPerson = await _context.People.FindAsync(person.PersonID);

            if (existingPerson == null) return false;

            _context.Entry(existingPerson).CurrentValues.SetValues(person);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteAsync(int personId)
        {
            var person = await _context.People.FindAsync(personId);

            if (person == null) return false;

            _context.People.Remove(person);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsByNationalNoAsync(string nationalNo)
        {
            return await _context.People.AnyAsync(p => p.NationalNo == nationalNo);
        }
        public async Task<bool> ExistsByIdAsync(int personId)
        {
            return await _context.People.AnyAsync(p => p.PersonID == personId);
        }
    }
}