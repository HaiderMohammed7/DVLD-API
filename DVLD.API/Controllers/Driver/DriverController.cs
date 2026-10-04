using DVLD.Application.Features.Driver.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Driver
{
    [ApiController]
    [Route("api/drivers")]
    [Authorize]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet("person/{personId}/local-licenses")]
        public async Task<IActionResult> GetLocalLicenses(int peronsId)
        {
            var result = await _driverService.GetDriverLocalLicensesAsync(peronsId);
            return Ok(result);
        }

        [HttpGet("person/{personId}/international-licenses")]
        public async Task<IActionResult> GetInternationalLicenses(int peronsId)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null) return NotFound();

            return Ok(driver);
        }
    }
}