using DVLD.Application.Features.Licenses.DTOs;
using DVLD.Application.Features.Licenses.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Licenses
{
    [ApiController]
    [Route("api/license")]
    [Authorize]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetForDetails(int id)
        {
            var license = await _licenseService.GetForDetailsAsync(id);
            if (license is null) return NotFound();

            return Ok(license);
        }

        [HttpPost("IssueDriverLicense")]
        public async Task<IActionResult> IssueDriverLicense(IssueDriverLicenseDto dto)
        {
            var licenseID = await _licenseService.IssueDriverLicenseAsync(dto);
            return Ok(licenseID);
        }

        [HttpPost("Detain")]
        public async Task<IActionResult> Detain(DetainLicenseDto dto)
        {
            var detainId = await _licenseService.DetainLicenseAsync(dto);
            if (detainId == -1) return BadRequest("License could not be detained.");

            return Ok(detainId);
        }
    }
}