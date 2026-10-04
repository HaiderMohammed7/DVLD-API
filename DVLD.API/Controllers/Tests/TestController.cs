using DVLD.Application.Features.Tests.DTOs;
using DVLD.Application.Features.Tests.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers.Tests
{
    [Authorize]
    [ApiController]
    [Route("api/tests")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost("conduct")]
        public async Task<IActionResult> Conduct(ConductTestDto dto)
        {
            await _testService.ConductTestAsync(dto);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var test = await _testService.GetByIdAsync(id);
            if (test is null) return NotFound();

            return Ok(test);
        }
    }
}