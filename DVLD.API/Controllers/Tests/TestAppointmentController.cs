using DVLD.Application.Features.Tests.DTOs;
using DVLD.Application.Features.Tests.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Tests
{
    [Authorize]
    [ApiController]
    [Route("api/test-appointments")]
    public class TestAppointmentController : ControllerBase
    {
        private readonly ITestAppointmentService _testAppointmentService;
        public TestAppointmentController(ITestAppointmentService testAppointmentService)
        {
            _testAppointmentService = testAppointmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testAppointment = await _testAppointmentService.GetByIdAsync(id);
            if (testAppointment is null) return NotFound();

            return Ok(testAppointment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _testAppointmentService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> Schedule(ScheduleTestDto dto)
        {
            var result = await _testAppointmentService.ScheduleTestAsync(dto);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateTestAppointmentDto dto)
        {
            var result = await _testAppointmentService.UpdateAppointmentDateAsync(dto);
            if (!result) return NotFound("Test Appointment not found.");

            return NoContent();
        }

        [HttpGet("schedule-info/{localDrivingLicenseApplicationId}/{testTypeId}")]
        public async Task<IActionResult> GetScheduleInfo(int localDrivingLicenseApplicationId, int testTypeId)
        {
            var result = await _testAppointmentService.GetScheduleTestInfoAsync(localDrivingLicenseApplicationId, testTypeId);
            if (result == null) return NotFound("Local Driving License Application not found.");

            return Ok(result);
        }

        [HttpGet("{testAppointmentId}/scheduled-info")]
        public async Task<IActionResult> GetScheduledTestInfo(int testAppointmentId)
        {
            var result = await _testAppointmentService.GetScheduledTestInfoAsync(testAppointmentId);
            if (result == null) return NotFound("Test Appointment not found.");

            return Ok(result);
        }
    }
}