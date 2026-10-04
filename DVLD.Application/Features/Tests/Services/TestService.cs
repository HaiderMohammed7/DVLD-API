using DVLD.Application.Features.Tests.DTOs;
using DVLD.Application.Features.Tests.Interfaces;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;
using DVLD.Domain.Entities;

namespace DVLD.Application.Features.Tests.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly ITestAppointmentRepository _testAppointmentRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public TestService(ITestRepository testRepository,ITestAppointmentRepository testAppointmentRepository,IUserService userService, IUnitOfWork unitOfWork)
        {
            _testRepository = testRepository;
            _testAppointmentRepository = testAppointmentRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task ConductTestAsync(ConductTestDto dto)
        {
            var appointment = await _testAppointmentRepository.GetByIdAsync(dto.TestAppointmentID);
            if (appointment == null) throw new Exception("Test Appointment not found.");
            if (appointment.IsLocked) throw new Exception("Cannot conduct a test for a locked appointment.");

            var existingTest = await _testRepository.GetByAppointmentIdAsync(dto.TestAppointmentID);
            if (existingTest != null) throw new Exception("This appointment already has a test result.");

            var user = await _userService.GetCurrentUserAsync();
            if (user == null || user.UserID == 0) throw new UnauthorizedAccessException("User is not found.");

            var test = new Test
            {
                TestAppointmentID = dto.TestAppointmentID,
                TestResult = dto.TestResult,
                Notes = dto.Notes,
                CreatedByUserID = user.UserID
            };

            await _testRepository.AddAsync(test);

            appointment.IsLocked = true;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetTestInfoDto> GetByIdAsync(int id)
        {
            var test = await _testRepository.GetByIdAsync(id);
            if (test == null) return null;

            return new GetTestInfoDto()
            {
                TestID = test.TestID,
                TestAppointmentID = test.TestAppointmentID,
                TestResult = test.TestResult,
                Notes = test.Notes,
                CreatedByUserID = test.CreatedByUserID
            };
        }
    }
}