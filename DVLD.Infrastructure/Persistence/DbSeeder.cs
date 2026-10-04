using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            var admin = await userRepository.GetByAuthUserIdAsync(5);
            if (admin != null) return;

            var newAdmin = new User
            {
                AuthUserId = 5,
                PersonID = 2,
                Role = UserRole.Admin,
                IsActive = true
            };

            await userRepository.AddAsync(newAdmin);
            await unitOfWork.SaveChangesAsync();
        }
    }
}