using DVLD.Application.Features.Licenses.DTOs;
using DVLD.Application.Features.Licenses.Interfaces;
using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Licenses
{
    [ApiController]
    [Route("api/licenses")]
    [Authorize]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        private readonly ILicenseClassService _licenseClassService;
        public LicenseController(ILicenseService licenseService, ILicenseClassService licenseClassService)
        {
            _licenseService = licenseService;
            _licenseClassService = licenseClassService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var license = await _licenseService.GetByIdAsync(id);
            if (license is null) return NotFound();

            return Ok(license);
        }

        [HttpPost("issue")]
        public async Task<IActionResult> Issue(IssueDriverLicenseDto dto)
        {
            var licenseID = await _licenseService.IssueAsync(dto);
            return Ok(licenseID);
        }

        [HttpPost("detain")]
        public async Task<IActionResult> Detain(DetainLicenseDto dto)
        {
            var detainId = await _licenseService.DetainAsync(dto);
            if (detainId == -1) return BadRequest("License could not be detained.");

            return Ok(detainId);
        }

        [HttpPost("release")]
        public async Task<IActionResult> Release(ReleaseDetainedLicenseDto dto)
        {
            var applicationId = await _licenseService.ReleaseAsync(dto.LicenseId);
            if (applicationId == -1) return BadRequest("License is not detained.");

            return Ok(applicationId);
        }

        [HttpPost("renew")]
        public async Task<IActionResult> Renew(RenewLocalDrivingLicenseDto dto)
        {
            var response = await _licenseService.RenewAsync(dto.LicenseId, dto.Notes);
            if (response == null) return BadRequest("License renewal failed.");

            return Ok(response);
        }

        [HttpPost("replace")]
        public async Task<IActionResult> Replace(ReplaceLicenseDto dto)
        {
            var response = await _licenseService.ReplaceAsync(dto);
            if (response == null) return BadRequest("License replacement failed.");

            return Ok(response);
        }

        [HttpGet("{id}/release-info")]
        public async Task<IActionResult> GetReleaseInfo(int id)
        {
            var license = await _licenseService.ReleaseInfo(id);
            if (license is null) return NotFound();

            return Ok(license);
        }

        [HttpGet("detained")]
        public async Task<IActionResult> GetDetainedLicenses()
        {
            var result = await _licenseService.GetDetainedList();
            return Ok(result);
        }

        [HttpGet("classes")]
        public async Task<IActionResult> GetLicenseClasses()
        {
            var result = await _licenseClassService.GetAllAsync();
            return Ok(result);
        }
    }
}