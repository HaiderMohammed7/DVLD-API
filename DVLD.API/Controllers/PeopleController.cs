using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/people")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IAuthorizationService _authorizationService;

        public PeopleController(IPersonService personService, IAuthorizationService authorizationService)
        {
            _personService = personService;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var auth = await _authorizationService.AuthorizeAsync(User, id, "OwnerOrAdmin");

            if (!auth.Succeeded)
                return Forbid();

            var result = await _personService.GetPersonByIdAsync(id);

            return result is null ? NotFound() : Ok(result);
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _personService.GetMyProfileAsync();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonDto request)
        {
            var result = await _personService.CreatePersonAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.PersonID }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePersonDto request)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            await _personService.UpdatePersonAsync(id, request);

            return NoContent();
        }
    }
}