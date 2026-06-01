using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public UserService(IUserRepository userRepository, ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<User> EnsureUserExistsAsync(int personId)
        {
            if (!_currentUser.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated");

            var authUserId = _currentUser.AuthUserId;

            var user = await _userRepository.GetByAuthUserIdAsync(authUserId);

            if (user != null)
            {
                if (user.PersonID != personId)
                    throw new Exception("This account is already linked to another person");

                return user;
            }

            var existingPersonUser = await _userRepository.GetByPersonIdAsync(personId);

            if (existingPersonUser != null)
                throw new Exception("Person already linked to another user");

            var newUser = new User
            {
                AuthUserId = authUserId,
                PersonID = personId,
                Role = UserRole.User,
                IsActive = true
            };

            await _userRepository.AddAsync(newUser);

            return newUser;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            if (!_currentUser.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var user = await _userRepository
                .GetByAuthUserIdAsync(_currentUser.AuthUserId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found in DVLD");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("User is inactive");

            return user;
        }

        private void EnsureAdmin(User user)
        {
            if (user.Role != UserRole.Admin)
                throw new UnauthorizedAccessException("Only Admin can perform this action");
        }

        private async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<int> CreateUserAsync(int newAuthUserId, int personId, UserRole role)
        {
            var currentUser = await GetCurrentUserAsync();
            EnsureAdmin(currentUser);

            if (newAuthUserId == _currentUser.AuthUserId)
                throw new Exception("You cannot create a user for yourself");

            var existingAuth = await _userRepository
                .GetByAuthUserIdAsync(newAuthUserId);

            if (existingAuth != null)
                throw new Exception("Auth user already exists");

            var existingPerson = await _userRepository
                .GetByPersonIdAsync(personId);

            if (existingPerson != null)
                throw new Exception("Person already linked to another user");

            var user = new User
            {
                AuthUserId = newAuthUserId,
                PersonID = personId,
                Role = role,
                IsActive = true
            };

            await _userRepository.AddAsync(user);

            return user.UserID;
        }

        public async Task ActivateUserAsync(int userId)
        {
            var currentUser = await GetCurrentUserAsync();
            EnsureAdmin(currentUser);

            var user = await GetUserByIdAsync(userId);

            if (user.IsActive)
                return;

            user.IsActive = true;

            await _userRepository.UpdateAsync();
        }

        public async Task DeactivateUserAsync(int userId)
        {
            var currentUser = await GetCurrentUserAsync();
            EnsureAdmin(currentUser);

            var user = await GetUserByIdAsync(userId);

            if (!user.IsActive)
                return;

            user.IsActive = false;

            await _userRepository.UpdateAsync();
        }

        public async Task AssignRoleAsync(int userId, UserRole role)
        {
            var currentUser = await GetCurrentUserAsync();
            EnsureAdmin(currentUser);

            var user = await GetUserByIdAsync(userId);

            user.Role = role;

            await _userRepository.UpdateAsync();
        }
    }
}