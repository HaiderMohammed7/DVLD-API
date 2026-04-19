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
            return await _context.Users.FindAsync(PersonId);
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }   
        public async Task<User?> GetByAuthUserIdAsync(int authUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.AuthUserId == authUserId);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}