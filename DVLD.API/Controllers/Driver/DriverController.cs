using DVLD.Application.Features.Driver.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Driver
{
    [ApiController]
    [Route("api/driver")]
    [Authorize]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet("driverLocalLicense/{peronsId}")]
        public async Task<IActionResult> GetDriverLocalLicensesAsync(int peronsId)
        {
            var result = await _driverService.GetDriverLocalLicensesAsync(peronsId);
            return Ok(result);
        }

        [HttpGet("driverInternationalLicense/{peronsId}")]
        public async Task<IActionResult> GetDriverInternationalLicensesAsync(int peronsId)
        {
            var result = await _driverService.GetDriverInternationalLicensesAsync(peronsId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _driverService.GetAllAsync();
            return Ok(result);
        }
    }
}