using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Auth.Interfaces;
using DVLD.Application.Features.Users.DTOs;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.HTTPClient;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IAuthApiClient _authApiClient;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, ICurrentUserService currentUser, IAuthApiClient authApiClient, IPersonRepository personRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _authApiClient = authApiClient;
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> EnsureExistsAsync(int personId)
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
            await _unitOfWork.SaveChangesAsync();

            return newUser;
        }

        public async Task<CurrentUserDto> GetCurrentUserAsync()
        {
            if (!_currentUser.IsAuthenticated) throw new UnauthorizedAccessException("User is not authenticated");

            var user = await _userRepository.GetByAuthUserIdAsync(_currentUser.AuthUserId);
            if (user == null) throw new UnauthorizedAccessException("User not found in DVLD");
            if (!user.IsActive) throw new UnauthorizedAccessException("User is inactive");

            return new CurrentUserDto()
            {
                UserID = user.UserID,
                PersonID = user.PersonID,
                AuthUserId = user.AuthUserId,
                Role = user.Role.ToString(),
            };
        }
        public async Task<UserDto?> GetByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserID,
                PersonId = user.PersonID,
                AuthUserId = user.AuthUserId,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
            };
        }
        public async Task<UserDto?> GetByPersonIdAsync(int personId)
        {
            var user = await _userRepository.GetByPersonIdAsync(personId);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserID,
                PersonId = user.PersonID,
                AuthUserId = user.AuthUserId,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
            };
        }
        public async Task<List<UserListDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var authUserIds = users.Select(u => u.AuthUserId).Distinct().ToList();

            var authUsers = await _authApiClient.GetBasicInfoAsync(authUserIds);

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
        public async Task<UserInfoDto?> GetInfoByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var authUser = await _authApiClient.GetByIdAsync(user.AuthUserId);

            return new UserInfoDto
            {
                UserId = user.UserID,
                PersonId = user.PersonID,
                UserName = authUser?.UserName ?? string.Empty,
                IsActive = user.IsActive
            };
        }

        public async Task<int> CreateAsync(CreateUserDto dto)
        {
            var person = await _personRepository.GetByIdAsync(dto.PersonId);
            if (person == null) throw new Exception("Person not found.");

            var existingPerson = await _userRepository.GetByPersonIdAsync(dto.PersonId);
            if (existingPerson != null) throw new Exception("Person already linked to another user");

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("CurrentUser not found.");

            var authUserId = await _authApiClient.RegisterAsync(new RegisterUserDto
            {
                userName = dto.UserName,
                email = dto.Email,
                password = dto.Password
            });

            var user = new User
            {
                AuthUserId = authUserId,
                PersonID = dto.PersonId,
                Role = UserRole.User,
                IsActive = dto.IsActive
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user.UserID;
        }
        public async Task UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            await _authApiClient.UpdateAsync(user.AuthUserId, new UpdateAuthUserDto
            {
                UserName = dto.UserName,
                Email = dto.Email,
            });
        }
        public async Task DeleteAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("CurrentUser not found.");

            if (user.UserID == currentUser.UserID) throw new Exception("You cannot delete your own account.");

            await _authApiClient.DeleteAsync(user.AuthUserId);

            await _userRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActivateAsync(int userId)
        {
            await ModifyUserStatusAsync(userId, u => u.IsActive = true);
        }
        public async Task DeactivateAsync(int userId)
        {
            await ModifyUserStatusAsync(userId, u => u.IsActive = false);
        }
        public async Task AssignRoleAsync(int userId, UserRole role)
        {
            await ModifyUserStatusAsync(userId, u => u.Role = role);
        }
        private async Task ModifyUserStatusAsync(int userId, Action<User> modifyAction)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            modifyAction(user);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int uersId)
        {
            return await _userRepository.IsExist(uersId);
        }
        public async Task<bool> ExistsByPersonIdAsync(int personId)
        {
            return await _userRepository.IsExistByPersonId(personId);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            await _authApiClient.ChangePasswordAsync(dto);
        }
    }
}