using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DVLD.Application.Features.InternationalLicense.Interfaces;

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
    }
}