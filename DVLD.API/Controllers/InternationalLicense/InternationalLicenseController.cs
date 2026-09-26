using DVLD.Application.Features.InternationalLicense.DTOs;
using DVLD.Application.Features.InternationalLicense.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.InternationalLicense
{
    [ApiController]
    [Route("api/inernationalLicense")]
    [Authorize]
    public class InternationalLicenseController : ControllerBase
    {
        private readonly IInternationalLicenseService _internationalLicenseService;
        public InternationalLicenseController(IInternationalLicenseService internationalLicenseService)
        {
            _internationalLicenseService = internationalLicenseService;
        }

        [HttpGet("Info/{internationalLicenseId}")]
        public async Task<IActionResult> GetInternationalLicenseInfo(int internationalLicenseId)
        {
            var result = await _internationalLicenseService.GetInternationalLicenseInfoAsync(internationalLicenseId);
            if (result == null) return NotFound("International License not found.");
            return Ok(result);
        }

        [HttpPost("Issue")]
        public async Task<IActionResult> IssueInternationalLicense(IssueInternationalLicenseDto dto)
        {
            var internationalLicenseId = await _internationalLicenseService.IssueInternationalLicenseAsync(dto.LocalLicenseID);
            return Ok(internationalLicenseId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _internationalLicenseService.GetAllAsync();
            return Ok(result);
        }
    }
}