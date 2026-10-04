using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;
using DVLD.Application.Features.LocalDrivingLicenseApplications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.LocalDrivingLicenseApplications
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
        public async Task<IActionResult> Update(int id, UpdateLocalDrivingLicenseApplicationDto dto)
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
            if (application is null) return NotFound();

            return Ok(application);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applications = await _localDrivingLicenseApplicationService.GetAllAsync();

            return Ok(applications);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _localDrivingLicenseApplicationService.CancelAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _localDrivingLicenseApplicationService.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var application = await _localDrivingLicenseApplicationService.GetInfoByIdAsync(id);

            if (application is null)
                return NotFound();

            return Ok(application);
        }

        [HttpGet("{id}/tests/{testTypeId}/active")]
        public async Task<IActionResult> HasActiveScheduledTest(int id, int testTypeId)
        {
            var result = await _localDrivingLicenseApplicationService.IsThereAnActiveScheduledTest(id, testTypeId);
            return Ok(result);
        }

        [HttpGet("{id}/tests/{testTypeId}/passed")]
        public async Task<IActionResult> HasPassedTestType(int id, int testTypeId)
        {
            var result = await _localDrivingLicenseApplicationService.DoesPassTestType(id, testTypeId);
            return Ok(result);
        }
    }
}