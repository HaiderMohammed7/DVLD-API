using DVLD.Application.Features.Auth.DTOs;
using DVLD.Application.Features.Tests.DTOs;
using DVLD.Application.Features.Tests.Services;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using Moq;

namespace DVLD.Tests.UnitTests.Services
{
    public class TestAppointmentServiceTests
    {
        private readonly Mock<ITestAppointmentRepository> _testAppointmentRepositoryMock;
        private readonly Mock<ILocalDrivingLicenseApplicationRepository> _ldlaRepositoryMock;
        private readonly Mock<ITestTypeRepository> _testTypeRepositoryMock;
        private readonly Mock<IApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<IApplicationTypeRepository> _applicationTypeRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public TestAppointmentServiceTests()
        {
            _testAppointmentRepositoryMock = new Mock<ITestAppointmentRepository>();
            _ldlaRepositoryMock = new Mock<ILocalDrivingLicenseApplicationRepository>();
            _testTypeRepositoryMock = new Mock<ITestTypeRepository>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _applicationTypeRepositoryMock = new Mock<IApplicationTypeRepository>();
            _userServiceMock = new Mock<IUserService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenCurrentUserIsNull_ShouldThrowUnauthorizedAccessException()
        {
            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync((CurrentUserDto?)null);

            _unitOfWorkMock
                .Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = 1,
                AppointmentDate = DateTime.Now
            };

            var act = () => service.ScheduleTestAsync(dto);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenLdlaDoesNotExist_ShouldThrowException()
        {
            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(new CurrentUserDto { UserID = 1});

            _ldlaRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((LocalDrivingLicenseApplication?)null);

            _unitOfWorkMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = 1,
                AppointmentDate = DateTime.Now
            };

            var act = () => service.ScheduleTestAsync(dto);

            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenTestTypeDoesNotExist_ShouldThrowException()
        {
            _userServiceMock .Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(new CurrentUserDto { UserID = 1});

            _ldlaRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new LocalDrivingLicenseApplication());

            _testTypeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((TestType?)null);

            _unitOfWorkMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = 1,
                AppointmentDate = DateTime.Now
            };

            var act = () => service.ScheduleTestAsync(dto);

            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenActiveAppointmentExists_ShouldThrowException()
        {
            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(new CurrentUserDto{ UserID = 1});

            _ldlaRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new LocalDrivingLicenseApplication());

            _testTypeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new TestType ());

            _testAppointmentRepositoryMock.Setup(x => x.IsThereAnActiveScheduledTest(1, 1)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = 1,
                AppointmentDate = DateTime.Now
            };

            var act = () => service.ScheduleTestAsync(dto);

            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenSchedulingWrittenTestWithoutPassingVision_ShouldThrowException()
        {
            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(new CurrentUserDto { UserID = 1 });

            _ldlaRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new LocalDrivingLicenseApplication());

            _testTypeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new TestType());

            _testAppointmentRepositoryMock.Setup(x => x.IsThereAnActiveScheduledTest(1, 2)).ReturnsAsync(false);

            _ldlaRepositoryMock.Setup(x => x.DoesPassTestType(1, (int)TestTypeEnum.Vision)).ReturnsAsync(false);

            _unitOfWorkMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = (int)TestTypeEnum.Written,
                AppointmentDate = DateTime.Now
            };

            var act = () => service.ScheduleTestAsync(dto);

            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenSchedulingStreetTestWithoutPassingWritten_ShouldThrowException()
        {
            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(new CurrentUserDto { UserID = 1 });

            _ldlaRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new LocalDrivingLicenseApplication());

            _testTypeRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new TestType());

            _testAppointmentRepositoryMock.Setup(x => x.IsThereAnActiveScheduledTest(1, 2)).ReturnsAsync(false);

            _ldlaRepositoryMock.Setup(x => x.DoesPassTestType(1, (int)TestTypeEnum.Vision)).ReturnsAsync(false);

            _ldlaRepositoryMock.Setup(x => x.DoesPassTestType(1, (int)TestTypeEnum.Written)).ReturnsAsync(false);

            _unitOfWorkMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = (int)TestTypeEnum.Written,
                AppointmentDate = DateTime.Now
            };

            var act = () => service.ScheduleTestAsync(dto);

            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenSchedulingFirstVisionTest_ShouldCreateAppointment()
        {
            // Arrange

            _userServiceMock
                .Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new CurrentUserDto
                {
                    UserID = 1
                });

            _ldlaRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new LocalDrivingLicenseApplication());

            _testTypeRepositoryMock
                .Setup(x => x.GetByIdAsync((int)TestTypeEnum.Vision))
                .ReturnsAsync(new TestType
                {
                    TestTypeID = (int)TestTypeEnum.Vision,
                    TestTypeFees = 10
                });

            _testAppointmentRepositoryMock
                .Setup(x => x.IsThereAnActiveScheduledTest(
                    1,
                    (int)TestTypeEnum.Vision))
                .ReturnsAsync(false);

            _testAppointmentRepositoryMock
                .Setup(x => x.GetByLDLAAndTestTypeAsync(
                    1,
                    (int)TestTypeEnum.Vision))
                .ReturnsAsync(new List<TestAppointment>());

            _unitOfWorkMock
                .Setup(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var appointmentDate = DateTime.Now.AddDays(1);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = (int)TestTypeEnum.Vision,
                AppointmentDate = appointmentDate
            };

            // Act

            var result = await service.ScheduleTestAsync(dto);

            // Assert

            Assert.NotNull(result);

            _testAppointmentRepositoryMock.Verify(
                x => x.AddAsync(It.Is<TestAppointment>(a =>
                    a.LocalDrivingLicenseApplicationID == 1 &&
                    a.TestTypeID == (int)TestTypeEnum.Vision &&
                    a.AppointmentDate == appointmentDate &&
                    a.PaidFees == 10 &&
                    a.CreatedByUserID == 1 &&
                    a.IsLocked == false &&
                    a.RetakeTestApplicationID == null)),
                Times.Once);
        }

        [Fact]
        public async Task ScheduleTestAsync_WhenPreviousTestFailed_ShouldCreateRetakeApplicationAndAppointment()
        {
            var currentUser = new CurrentUserDto {  UserID = 1};

            var previousAppointment = new TestAppointment
            {
                TestAppointmentID = 10,
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = (int)TestTypeEnum.Vision,
                Tests = new List<Test> { new Test { TestID = 20, TestResult = false }} 
            };

            _userServiceMock
                .Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(currentUser);

            _ldlaRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new LocalDrivingLicenseApplication
                {
                    Applications = new Applications { ApplicantPersonID = 50 }
                });

            _testTypeRepositoryMock
                .Setup(x => x.GetByIdAsync((int)TestTypeEnum.Vision))
                .ReturnsAsync(new TestType
                {
                    TestTypeID = (int)TestTypeEnum.Vision,
                    TestTypeFees = 10
                });

            _testAppointmentRepositoryMock
                .Setup(x => x.IsThereAnActiveScheduledTest(
                    1,
                    (int)TestTypeEnum.Vision))
                .ReturnsAsync(false);

            _testAppointmentRepositoryMock
                .Setup(x => x.GetByLDLAAndTestTypeAsync(
                    1,
                    (int)TestTypeEnum.Vision))
                .ReturnsAsync(new List<TestAppointment>{ previousAppointment });

            _applicationTypeRepositoryMock
                .Setup(x => x.GetByIdAsync((int)ApplicationTypeEnum.RetakeTest))
                .ReturnsAsync(new ApplicationType
                {
                    ApplicationTypeID = (int)ApplicationTypeEnum.RetakeTest,
                    ApplicationFees = 5
                });

            _applicationRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Applications>()))
                .Callback<Applications>(application =>
                {
                    application.ApplicationID = 100;
                });

            _testAppointmentRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<TestAppointment>()))
                .Callback<TestAppointment>(appointment =>
                {
                    appointment.TestAppointmentID = 200;
                });

            _unitOfWorkMock
                .Setup(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task<ScheduleTestResultDto>>>()))
                .Returns((Func<Task<ScheduleTestResultDto>> action) => action());

            var service = new TestAppointmentService(
                _applicationRepositoryMock.Object,
                _applicationTypeRepositoryMock.Object,
                _testAppointmentRepositoryMock.Object,
                _ldlaRepositoryMock.Object,
                _testTypeRepositoryMock.Object,
                _userServiceMock.Object,
                _unitOfWorkMock.Object);

            var appointmentDate = DateTime.Now.AddDays(1);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = 1,
                TestTypeID = (int)TestTypeEnum.Vision,
                AppointmentDate = appointmentDate
            };

            var result = await service.ScheduleTestAsync(dto);

            Assert.NotNull(result);

            Assert.Equal(200, result.TestAppointmentID);

            _applicationRepositoryMock.Verify(
                x => x.AddAsync(It.Is<Applications>(application =>
                    application.ApplicantPersonID == 50 &&
                    application.ApplicationTypeID == (int)ApplicationTypeEnum.RetakeTest &&
                    application.PaidFees == 5 &&
                    application.ApplicationStatus == ApplicationStatus.New &&
                    application.CreatedByUserID == 1)),
                Times.Once);

            _testAppointmentRepositoryMock.Verify(
                x => x.AddAsync(It.Is<TestAppointment>(appointment =>
                    appointment.LocalDrivingLicenseApplicationID == 1 &&
                    appointment.TestTypeID == (int)TestTypeEnum.Vision &&
                    appointment.AppointmentDate == appointmentDate &&
                    appointment.PaidFees == 10 &&
                    appointment.CreatedByUserID == 1 &&
                    appointment.IsLocked == false &&
                    appointment.RetakeTestApplicationID == 100)),
                Times.Once);
        }
    }
}