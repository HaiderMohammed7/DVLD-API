using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [ApiController]
    [Route("api/license-class")]
    [Authorize]
    public class LicenseClassController : ControllerBase
    {
        private readonly ILicenseClassService _licenseClassService;

        public LicenseClassController(ILicenseClassService licenseClassService)
        {
            _licenseClassService = licenseClassService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _licenseClassService.GetAllAsync();

            return Ok(result);
        }
    }
}