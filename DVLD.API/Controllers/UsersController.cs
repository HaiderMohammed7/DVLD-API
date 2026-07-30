using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UsersController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }



        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userService.GetCurrentUserAsync();

            return Ok(new CurrentUserDto
            {
                UserID = user.UserID,
                PersonID = user.PersonID,
                AuthUserId = user.AuthUserId
            });
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, user, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(user);
        }

        [HttpGet("person/{id}")]
        public async Task<IActionResult> GetByPersonId(int id)
        {
            var user = await _userService.GetUserByPersonIdAsync(id);

            if (user is null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, user, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(user);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();

            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("withUsername/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }



        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var userId = await _userService.CreateUserAsync(dto);

            return Ok(userId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, user, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            await _userService.UpdateAsync(id, dto);

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            await _userService.DeleteAsync(id);

            return NoContent();
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> ExistsById(int id)
        {
            var exists = await _userService.ExistsByIdAsync(id);

            return Ok(exists);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("PersonId/{PersonId}/exists")]
        public async Task<IActionResult> ExistsByPersonId(int PersonId)
        {
            var exists = await _userService.ExistsByPersonIdAsync(PersonId);

            return Ok(exists);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            await _userService.ChangePasswordAsync(dto);

            return Ok();
        }
    }
}