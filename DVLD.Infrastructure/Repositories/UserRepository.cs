using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DVLDDbContext _context;
        public UserRepository(DVLDDbContext context)
        {
            _context = context;
        }


        public async Task<User?> GetByPersonIdAsync(int PersonId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PersonID == PersonId);
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }   
        public async Task<User?> GetByAuthUserIdAsync(int authUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.AuthUserId == authUserId);
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.Include(u => u.Person).AsNoTracking().ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserExist(int userId)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.UserID == userId);
        }
        public async Task<bool> IsUserExistForPersonId(int personId)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.PersonID == personId);
        }
    }
}