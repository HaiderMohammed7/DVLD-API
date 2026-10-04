using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DVLDDbContext _context;
        public UserRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByPersonIdAsync(int personId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PersonID == personId);
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }   
        public async Task<User?> GetByAuthUserIdAsync(int authUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.AuthUserId == authUserId);
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Person).AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(User user)
        {
           await _context.Users.AddAsync(user);
        }
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> IsExist(int userId)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.UserID == userId);
        }
        public async Task<bool> IsExistByPersonId(int personId)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.PersonID == personId);
        }
    }
}