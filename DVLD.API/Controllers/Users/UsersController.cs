using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Users.DTOs;
using DVLD.Application.Features.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Users
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
            return Ok(await _userService.GetCurrentUserAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user is null) return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, user, "OwnerOrAdmin");
            if (!authResult.Succeeded) return Forbid();

            return Ok(user);
        }

        [HttpGet("person/{personId}")]
        public async Task<IActionResult> GetByPersonId(int personId)
        {
            var user = await _userService.GetByPersonIdAsync(personId);
            if (user is null) return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, user, "OwnerOrAdmin");
            if (!authResult.Succeeded) return Forbid();

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
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetInfo(int id)
        {
            var user = await _userService.GetInfoByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var userId = await _userService.CreateAsync(dto);
            return Ok(userId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user is null) return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, user, "OwnerOrAdmin");
            if (!authResult.Succeeded) return Forbid();

            await _userService.UpdateAsync(id, dto);

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

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
        [HttpGet("person/{personId}/exists")]
        public async Task<IActionResult> ExistsByPersonId(int personId)
        {
            var exists = await _userService.ExistsByPersonIdAsync(personId);
            return Ok(exists);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            await _userService.ActivateAsync(id);

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _userService.DeactivateAsync(id);

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> AssignRole(int id, AssignUserRoleDto dto)
        {
            await _userService.AssignRoleAsync(id, dto.Role);

            return NoContent();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            await _userService.ChangePasswordAsync(dto);
            return Ok();
        }
    }
}