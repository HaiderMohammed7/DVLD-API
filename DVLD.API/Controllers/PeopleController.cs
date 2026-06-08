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
            var person = await _personService.GetByIdAsync(id);

            if (person is null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, person, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(person);
        }

        [HttpGet("national-no/{nationalNo}")]
        public async Task<IActionResult> GetByNationalNo(string nationalNo)
        {
            var person = await _personService.GetByNationalNoAsync(nationalNo);

            if (person is null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, person, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(person);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _personService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _personService.GetMyProfileAsync();

            return result is null ? NotFound() : Ok(result);
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonDto request)
        {
            var result = await _personService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.PersonID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePersonDto request)
        {
            var person = await _personService.GetByIdAsync(id);

            if (person is null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, person, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            await _personService.UpdateAsync(id, request);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _personService.GetByIdAsync(id);

            if (person is null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, person, "OwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            var deleted = await _personService.DeleteAsync(id);

            return NoContent();
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> ExistsById(int id)
        {
            var exists = await _personService.ExistsByIdAsync(id);

            return Ok(exists);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("national-no/{nationalNo}/exists")]
        public async Task<IActionResult> ExistsByNationalNo(string nationalNo)
        {
            var exists = await _personService.ExistsByNationalNoAsync(nationalNo);

            return Ok(exists);
        }

        [HttpGet("{id}/CountryName")]
        public async Task<IActionResult> GetCountryName(int id)
        {
            var result = await _personService.GetCountryNameByIdAsync(id);

            return Ok(result);
        }
    }
}