using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [ApiController]
    [Route("api/test-types")]
    [Authorize]
    public class TestTypeController : ControllerBase
    {
        private readonly ITestTypeService _testTypeService;

        public TestTypeController(ITestTypeService testTypeService)
        {
            _testTypeService = testTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var testType = await _testTypeService.GetAllAsync();

            return Ok(testType);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testType = await _testTypeService.GetByIdAsync(id);

            if (testType is null)
                return NotFound();

            return Ok(testType);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTestTypeDto dto)
        {
            var testType = await _testTypeService.GetByIdAsync(id);

            if (testType is null)
                return NotFound();

            await _testTypeService.UpdateAsync(id, dto);

            return NoContent();
        }
    }
}