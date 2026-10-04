using DVLD.Application.Features.InternationalLicense.DTOs;
using DVLD.Application.Features.InternationalLicense.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.InternationalLicense
{
    [ApiController]
    [Route("api/international-licenses")]
    [Authorize]
    public class InternationalLicenseController : ControllerBase
    {
        private readonly IInternationalLicenseService _internationalLicenseService;
        public InternationalLicenseController(IInternationalLicenseService internationalLicenseService)
        {
            _internationalLicenseService = internationalLicenseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _internationalLicenseService.GetInternationalLicenseInfoAsync(id);
            if (result == null) return NotFound("International License not found.");
            return Ok(result);
        }

        [HttpPost("issue")]
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