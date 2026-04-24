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

        public async Task<Person?> GetPersonByIdAsync(int personId)
        {
            return await _context.People.FindAsync(personId);
        }

        public async Task<Person> AddAsync(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            return person;
        }

        public async Task UpdateAsync(Person person)
        {
            _context.People.Update(person);
            await _context.SaveChangesAsync();
        }
    }
}