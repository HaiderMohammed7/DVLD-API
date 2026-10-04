using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Features.Tests.DTOs;
using DVLD.Application.Features.Tests.Interfaces;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using ApplicationEntity = DVLD.Domain.Entities.Applications;

namespace DVLD.Application.Features.Tests.Services
{
    public class TestAppointmentService : ITestAppointmentService
    {
        private readonly ITestAppointmentRepository _testAppointmentRepository;
        private readonly ILocalDrivingLicenseApplicationRepository _ldlaRepository;
        private readonly ITestTypeRepository _testTypeRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationTypeRepository _applicationTypeRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public TestAppointmentService(IApplicationRepository applicationRepository, IApplicationTypeRepository applicationTypeRepository, ITestAppointmentRepository testAppointmentRepository,ILocalDrivingLicenseApplicationRepository ldlaRepository,ITestTypeRepository testTypeRepository, IApplicationService applicationService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _applicationRepository = applicationRepository;
            _applicationTypeRepository = applicationTypeRepository;
            _testAppointmentRepository = testAppointmentRepository;
            _ldlaRepository = ldlaRepository;
            _testTypeRepository = testTypeRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<TestAppointmentDTO?> GetByIdAsync(int id)
        {
            var appointment = await _testAppointmentRepository.GetByIdAsync(id);
            if (appointment == null) return null;

            return new TestAppointmentDTO
            {
                PaidFees = appointment.PaidFees,
                AppointmentDate = appointment.AppointmentDate,
                RetakeTestApplicationID = appointment.RetakeTestApplicationID,
                RetakeTestPaidFees = appointment.Applications?.PaidFees,
                IsLocked = appointment.IsLocked
            };
        }
        public async Task<List<AppointmentsListDto>> GetAllAsync()
        {
            var list = await _testAppointmentRepository.GetAllAsync();

            return list.Select(item => new AppointmentsListDto
            {
                AppointmentId = item.TestAppointmentID,
                AppointmentDate = item.AppointmentDate,
                PaidFees = item.PaidFees,
                IsLocked= item.IsLocked
            }).ToList();
        }
        public async Task<GetScheduleTestInfoDto?> GetScheduleTestInfoAsync(int localDrivingLicenseApplicationID, int testTypeID)
        {
            var info = await _ldlaRepository.GetScheduleTestInfoAsync(localDrivingLicenseApplicationID, testTypeID);
            if (info == null) return null;

            info.RetakeApplicationFees = info.Trial > 0 ? 5 : 0;
            info.TotalFees = info.TestFees + info.RetakeApplicationFees;

            return info;
        }
        public async Task<GetScheduledTestInfoDto?> GetScheduledTestInfoAsync(int testAppointmentID)
        {
            return await _testAppointmentRepository.GetScheduledTestInfoAsync(testAppointmentID);
        }

        public async Task<ScheduleTestResultDto> ScheduleTestAsync(ScheduleTestDto dto)
        {
            var response = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null || currentUser.UserID == 0) throw new UnauthorizedAccessException("User is not found.");

                var ldla = await _ldlaRepository.GetByIdAsync(dto.LocalDrivingLicenseApplicationID);
                if (ldla == null) throw new Exception("Local Driving License Application not found.");

                var testType = await _testTypeRepository.GetByIdAsync(dto.TestTypeID);
                if (testType == null) throw new Exception("Test Type not found.");

                var hasActiveAppointment = await _testAppointmentRepository.IsThereAnActiveScheduledTest(dto.LocalDrivingLicenseApplicationID, dto.TestTypeID);
                if (hasActiveAppointment) throw new Exception("There is already an active appointment for this test type.");

                if (dto.TestTypeID == (int)TestTypeEnum.Written)
                {
                    var passedVision = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID, (int)TestTypeEnum.Vision);
                    if (!passedVision) throw new Exception("The applicant must pass the Vision Test first.");
                }

                if (dto.TestTypeID == (int)TestTypeEnum.Street)
                {
                    var passedWritten = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID, (int)TestTypeEnum.Written);
                    if (!passedWritten) throw new Exception("The applicant must pass the Written Test first.");
                }

                var previousAppointments = await _testAppointmentRepository.GetByLDLAAndTestTypeAsync(dto.LocalDrivingLicenseApplicationID, dto.TestTypeID);
                var isFirstTime = !previousAppointments.Any();

                int? retakeApplicationId = null;

                if (!isFirstTime)
                {
                    var lastAppointment = previousAppointments.First();

                    var lastTest = lastAppointment.Tests.OrderByDescending(x => x.TestID).FirstOrDefault();
                    if (lastTest == null) throw new Exception("The previous appointment has not been completed yet.");
                    if (lastTest.TestResult) throw new Exception("The applicant has already passed this test type.");

                    var applicationType = await _applicationTypeRepository.GetByIdAsync((int)ApplicationTypeEnum.RetakeTest);
                    if (applicationType == null) throw new Exception("Application type not found.");

                    var retakeApplication = new ApplicationEntity
                    {
                        ApplicantPersonID = ldla.Applications!.ApplicantPersonID,
                        ApplicationTypeID = applicationType.ApplicationTypeID,
                        PaidFees = applicationType.ApplicationFees,

                        ApplicationDate = DateTime.UtcNow,
                        LastStatusDate = DateTime.UtcNow,
                        ApplicationStatus = ApplicationStatus.New,
                        CreatedByUserID = currentUser.UserID
                    };

                    await _applicationRepository.AddAsync(retakeApplication);

                    retakeApplicationId = retakeApplication.ApplicationID;
                }

                var appointment = new TestAppointment
                {
                    TestTypeID = dto.TestTypeID,
                    LocalDrivingLicenseApplicationID = dto.LocalDrivingLicenseApplicationID,
                    AppointmentDate = dto.AppointmentDate,
                    PaidFees = testType.TestTypeFees,
                    CreatedByUserID = currentUser.UserID,
                    IsLocked = false,
                    RetakeTestApplicationID = retakeApplicationId
                };

                await _testAppointmentRepository.AddAsync(appointment);

                return new ScheduleTestResultDto
                {
                    TestAppointmentID = appointment.TestAppointmentID
                };
            });

            return response;          
        }
        public async Task<bool> UpdateAppointmentDateAsync(UpdateTestAppointmentDto dto)
        {
            var appointment = await _testAppointmentRepository.GetByIdAsync(dto.TestAppointmentID);
            if (appointment == null) return false;
            if (appointment.IsLocked) throw new Exception("Cannot update a locked test appointment.");

            appointment.AppointmentDate = dto.AppointmentDate;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }     
    }
}