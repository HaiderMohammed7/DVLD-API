using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IUserRepository userRepository)
        {
            var admin = await userRepository.GetByAuthUserIdAsync(5);

            if (admin != null)
                return;

            var newAdmin = new User
            {
                AuthUserId = 5,
                PersonID = 2,
                Role = UserRole.Admin,
                IsActive = true
            };

            await userRepository.AddAsync(newAdmin);
        }
    }
}