using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [ApiController]
    [Route("api/local-driving-license-applications")]
    [Authorize]
    public class LocalDrivingLicenseApplicationController : ControllerBase
    {
        private readonly ILocalDrivingLicenseApplicationService _localDrivingLicenseApplicationService;

        public LocalDrivingLicenseApplicationController(ILocalDrivingLicenseApplicationService localDrivingLicenseApplicationService)
        {
            _localDrivingLicenseApplicationService = localDrivingLicenseApplicationService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateLocalDrivingLicenseApplicationDto dto)
        {
            var id = await _localDrivingLicenseApplicationService.AddAsync(dto);

            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update( int id, UpdateLocalDrivingLicenseApplicationDto dto)
        {
            try
            {
                await _localDrivingLicenseApplicationService.UpdateAsync(id, dto);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var application = await _localDrivingLicenseApplicationService.GetByIdAsync(id);

            if (application is null)
                return NotFound();

            return Ok(application);
        }
    }
}