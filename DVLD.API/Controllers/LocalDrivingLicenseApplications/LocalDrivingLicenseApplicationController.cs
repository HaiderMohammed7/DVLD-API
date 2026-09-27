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

            if (application is null)
                return NotFound();

            return Ok(application);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applications = await _localDrivingLicenseApplicationService.GetAllAsync();

            return Ok(applications);
        }

        [HttpPut("Cancel/{id}")]
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

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetForDetails(int id)
        {
            var application = await _localDrivingLicenseApplicationService.GetForDetailsAsync(id);

            if (application is null)
                return NotFound();

            return Ok(application);
        }

        [HttpGet("IsThereAnActiveScheduledTest")]
        public async Task<IActionResult> IsThereAnActiveScheduledTest(int ldlaId, int testTypeID)
        {
            bool result = await _localDrivingLicenseApplicationService.IsThereAnActiveScheduledTest(ldlaId, testTypeID);

            return Ok(result);
        }

        [HttpGet("DoesPassTestType")]
        public async Task<IActionResult> DoesPassTestType(int ldlaId, int testTypeID)
        {
            bool result = await _localDrivingLicenseApplicationService.DoesPassTestType(ldlaId, testTypeID);

            return Ok(result);
        }

        [HttpPost("Renew")]
        public async Task<IActionResult> Renew(RenewLocalDrivingLicenseDto dto)
        {
            var response = await _localDrivingLicenseApplicationService.RenewLocalDrivingLicenseAsync(dto.LicenseID,dto.Notes);
            if (response == null) return BadRequest("License renewal failed.");

            return Ok(response);
        }
    }
}