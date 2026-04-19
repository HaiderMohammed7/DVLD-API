using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DVLD.Application.DTOs;

namespace DVLD.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var userId = await _userService.CreateUserAsync(
                dto.AuthUserId,
                dto.PersonId,
                dto.Role);

            return Ok(new { userId });
        }

        [HttpPost("me")]
        public async Task<IActionResult> RegisterMe([FromBody] RegisterMeDto dto)
        {
            var user = await _userService.EnsureUserExistsAsync(dto.PersonId);

            return Ok(new
            {
                userId = user.UserID,
                role = user.Role,
                isActive = user.IsActive
            });
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            await _userService.ActivateUserAsync(id);
            return NoContent();
        }
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _userService.DeactivateUserAsync(id);
            return NoContent();
        }


        [HttpPut("{id}/role")]
        public async Task<IActionResult> AssignRole(int id, AssignRoleDto dto)
        {
            await _userService.AssignRoleAsync(id, dto.Role);
            return NoContent();
        }
    }
}