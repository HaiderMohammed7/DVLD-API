using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IAuthApiClient _authApiClient;

        public UserService(IUserRepository userRepository, ICurrentUserService currentUser,
            IAuthApiClient authApiClient)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _authApiClient = authApiClient;
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
        private void EnsureAdmin(User user)
        {
            if (user.Role != UserRole.Admin)
                throw new UnauthorizedAccessException("Only Admin can perform this action");
        }

        public async Task<User> GetCurrentUserAsync()
        {
            if (!_currentUser.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var user = await _userRepository.GetByAuthUserIdAsync(_currentUser.AuthUserId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found in DVLD");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("User is inactive");

            return user;
        }
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            return MapToDto(user);
        }
        public async Task<UserDto> GetUserByPersonIdAsync(int personID)
        {
            var user = await _userRepository.GetByPersonIdAsync(personID);

            if (user == null)
                throw new KeyNotFoundException($"User with Person ID {personID} not found.");

            return MapToDto(user);
        }
        public async Task<List<UserListDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllUsers();

            var authUserIds = users.Select(u => u.AuthUserId).Distinct().ToList();

            var authUsers = await _authApiClient.GetUsersBasicInfoAsync(authUserIds);

            var result = users.Select(user =>
            {
                var authUser = authUsers.FirstOrDefault(a => a.UserId == user.AuthUserId);

                return new UserListDto
                {
                    UserId = user.UserID,
                    PersonId = user.PersonID,

                    FullName = $"{user.Person.FirstName} " +
                               $"{user.Person.SecondName} " +
                               $"{user.Person.ThirdName} " +
                               $"{user.Person.LastName}",

                    UserName = authUser?.UserName ?? "",

                    IsActive = user.IsActive
                };
            }).ToList();

            return result;
        }
        public async Task<UserInfoDto?> GetByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return null;

            var authUser = await _authApiClient.GetUserByIdAsync(user.AuthUserId);

            return new UserInfoDto
            {
                UserId = user.UserID,
                PersonId = user.PersonID,
                UserName = authUser?.UserName ?? string.Empty,
                IsActive = user.IsActive
            };
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
        public async Task<bool> UpdateAsync(int userId, User request)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            user.AuthUserId = request.AuthUserId;
            user.PersonID = request.PersonID;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            return await _userRepository.UpdateAsync();
        }
        public async Task<bool> DeleteAsync(int userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }
        
        public async Task ActivateUserAsync(int userId)
        {
            await ModifyUserStatusAsync(userId, u => u.IsActive = true);
        }
        public async Task DeactivateUserAsync(int userId)
        {
            await ModifyUserStatusAsync(userId, u => u.IsActive = false);
        }
        public async Task AssignRoleAsync(int userId, UserRole role)
        {
            await ModifyUserStatusAsync(userId, u => u.Role = role);
        }
        private async Task ModifyUserStatusAsync(int userId, Action<UserDto> modifyAction)
        {
            var currentUser = await GetCurrentUserAsync();
            EnsureAdmin(currentUser);

            var user = await GetUserByIdAsync(userId);
            modifyAction(user);

            await _userRepository.UpdateAsync();
        }

        public async Task<bool> ExistsByIdAsync(int uersId)
        {
            return await _userRepository.IsUserExist(uersId);
        }
        public async Task<bool> ExistsByPersonIdAsync(int personId)
        {
            return await _userRepository.IsUserExistForPersonId(personId);
        }

        private static UserDto MapToDto(User u)
        {
            return new UserDto
            {
                UserID = u.UserID,
                PersonID = u.PersonID,
                AuthUserId = u.AuthUserId,
                Role = u.Role,
                IsActive = u.IsActive,
            };
        }
    }
}