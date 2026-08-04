using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [ApiController]
    [Route("api/application-types")]
    [Authorize]
    public class ApplicationTypesController : ControllerBase
    {
        private readonly IApplicationTypeService _applicationTypeService;

        public ApplicationTypesController(IApplicationTypeService applicationTypeService)
        {
            _applicationTypeService = applicationTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applicationTypes = await _applicationTypeService.GetAllAsync();

            return Ok(applicationTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var applicationType = await _applicationTypeService.GetByIdAsync(id);

            if (applicationType is null)
                return NotFound();

            return Ok(applicationType);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateApplicationTypeDto dto)
        {
            var applicationType = await _applicationTypeService.GetByIdAsync(id);

            if (applicationType is null)
                return NotFound();

            await _applicationTypeService.UpdateAsync(id, dto);

            return NoContent();
        }
    }
}