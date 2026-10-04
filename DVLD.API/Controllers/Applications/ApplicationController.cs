using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Applications
{
    [Authorize]
    [ApiController]
    [Route("api/applications")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicationTypeService _applicationTypeService;

        public ApplicationController(IApplicationService applicationService, IApplicationTypeService applicationTypeService)
        {
            _applicationService = applicationService;
            _applicationTypeService = applicationTypeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var application = await _applicationService.GetByIdAsync(id);
            if (application is null) return NotFound();

            return Ok(application);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetApplicationTypes()
        {
            var applicationTypes = await _applicationTypeService.GetAllAsync();
            return Ok(applicationTypes);
        }

        [HttpGet("types/{id}")]
        public async Task<IActionResult> GetApplicationTypeById(int id)
        {
            var applicationType = await _applicationTypeService.GetByIdAsync(id);
            if (applicationType is null) return NotFound();

            return Ok(applicationType);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("types/{id}")]
        public async Task<IActionResult> UpdateApplicationType(int id, UpdateApplicationTypeDto dto)
        {
            var applicationType = await _applicationTypeService.GetByIdAsync(id);
            if (applicationType is null) return NotFound();

            await _applicationTypeService.UpdateAsync(id, dto);

            return NoContent();
        }
    }
}