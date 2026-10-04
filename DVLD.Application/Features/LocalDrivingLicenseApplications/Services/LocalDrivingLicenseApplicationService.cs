using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;
using DVLD.Application.Features.LocalDrivingLicenseApplications.Interfaces;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using ApplicationEntity = DVLD.Domain.Entities.Applications;

namespace DVLD.Application.Features.LocalDrivingLicenseApplications.Services
{
    public class LocalDrivingLicenseApplicationService : ILocalDrivingLicenseApplicationService
    {
        private readonly IUserService _userService;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILocalDrivingLicenseApplicationRepository _localRepository;
        private readonly ILicenseClassRepository _licenseClassRepository;
        private readonly IApplicationTypeRepository _applicationTypeRepository;
        private readonly ITestAppointmentRepository _testAppointmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocalDrivingLicenseApplicationService(IUserService userService, IApplicationRepository applicationRepository, ILocalDrivingLicenseApplicationRepository localRepository, ILicenseClassRepository licenseClassRepository, IApplicationTypeRepository applicationTypeRepository, ITestAppointmentRepository testAppointmentRepository, ILicenseRepository licenseRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _applicationRepository = applicationRepository;
            _localRepository = localRepository;
            _licenseClassRepository = licenseClassRepository;
            _applicationTypeRepository = applicationTypeRepository;
            _testAppointmentRepository = testAppointmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetLocalDrivingLicenseApplicationDto?> GetByIdAsync(int id)
        {
            var application = await _localRepository.GetByIdAsync(id);
            if (application is null) return null;

            return new GetLocalDrivingLicenseApplicationDto
            {
                LocalDrivingLicenseApplicationID = application.LocalDrivingLicenseApplicationID,

                PersonId = application.Applications.ApplicantPersonID,

                ApplicationDate = application.Applications.ApplicationDate,

                LicenseClassId = application.LicenseClassID,

                PaidFees = application.Applications.PaidFees,

                CreatedByUserID = application.Applications.CreatedByUserID
            };
        }
        public async Task<GetLocalDrivingLicenseApplicationInfoDto?> GetInfoByIdAsync(int id)
        {
            var application = await _localRepository.GetByIdAsync(id);
            if (application is null) return null;

            return new GetLocalDrivingLicenseApplicationInfoDto
            {
                ApplicationID = application.ApplicationID,

                LicenseID = application.Applications.Licenses.FirstOrDefault(x => x.IsActive)?.LicenseID,

                LicenseClassName = application.LicenseClass.ClassName,

                PassedTests = application.TestAppointments.SelectMany(x => x.Tests).Count(x => x.TestResult)
            };
        }
        public async Task<List<GetAllLocalDrivingLicenseApplicationDto>> GetAllAsync()
        {
            var applications = await _localRepository.GetAllAsync();

            return applications.Select(application => new GetAllLocalDrivingLicenseApplicationDto
            {
                LocalDrivingLicenseApplicationID = application.LocalDrivingLicenseApplicationID,

                DrivingClass = application.LicenseClass!.ClassName,

                NationalNo = application.Applications!.Person!.NationalNo,

                FullName = application.Applications.Person.FirstName + " " +
                application.Applications.Person.SecondName + " " +
                application.Applications.Person.ThirdName + " " +
                application.Applications.Person.LastName,

                ApplicationDate = application.Applications.ApplicationDate,

                PassedTests = application.TestAppointments.SelectMany(x => x.Tests).Count(x => x.TestResult),

                Status = application.Applications.ApplicationStatus.ToString()
            }).ToList();
        }

        public async Task<int> AddAsync(CreateLocalDrivingLicenseApplicationDto dto)
        {
            var id = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var licenseClass = await _licenseClassRepository.GetByIdAsync(dto.LicenseClassId);
                if (licenseClass is null) throw new Exception("License class not found.");

                bool hasActive = await _localRepository.HasActiveApplicationAsync(dto.PersonId, dto.LicenseClassId);
                if (hasActive) throw new Exception("Person already has an active application for this license class.");

                var applicationType = await _applicationTypeRepository.GetByIdAsync((int)ApplicationTypeEnum.NewLocalDrivingLicense);

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null) throw new Exception("Current user not found.");

                var application = new ApplicationEntity
                {
                    ApplicantPersonID = dto.PersonId,
                    ApplicationTypeID = (int)ApplicationTypeEnum.NewLocalDrivingLicense,
                    PaidFees = applicationType.ApplicationFees,

                    ApplicationDate = DateTime.UtcNow,
                    LastStatusDate = DateTime.UtcNow,
                    ApplicationStatus = ApplicationStatus.New,
                    CreatedByUserID = currentUser.UserID
                };

                await _applicationRepository.AddAsync(application);

                var ldla = new LocalDrivingLicenseApplication
                {
                    ApplicationID = application.ApplicationID,
                    LicenseClassID = dto.LicenseClassId
                };

                await _localRepository.AddAsync(ldla);

                return ldla.LocalDrivingLicenseApplicationID;
            });

            return id;
        }
        public async Task UpdateAsync(int localDrivingLicenseApplicationId, UpdateLocalDrivingLicenseApplicationDto dto)
        {
            var application = await _localRepository.GetByIdAsync(localDrivingLicenseApplicationId);
            if (application is null) throw new Exception("Local driving license application not found.");

            var licenseClass = await _licenseClassRepository.GetByIdAsync(dto.LicenseClassId);
            if (licenseClass is null) throw new Exception("License class not found.");

            int personId = application.Applications.ApplicantPersonID;

            bool hasActive = await _localRepository.HasActiveApplicationAsync(personId, dto.LicenseClassId, localDrivingLicenseApplicationId);
            if (hasActive) throw new Exception("Person already has an active application for this license class.");

            application.LicenseClassID = dto.LicenseClassId;

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var ldla = await _localRepository.GetByIdAsync(id);
                if (ldla is null) throw new Exception("Application not found.");

                var application = await _applicationRepository.GetByIdAsync(ldla.ApplicationID);
                if (application is null) throw new Exception("Application not found.");

                if (ldla.Applications.ApplicationStatus != ApplicationStatus.New)
                    throw new Exception("Only new applications can be deleted.");

                bool hasAppointments = await _testAppointmentRepository.HasTestAppointmentsAsync(id);

                if (hasAppointments) throw new Exception("Cannot delete an application that has test appointments.");

                await _localRepository.DeleteAsync(ldla);

                await _applicationRepository.DeleteAsync(application);
            });   
        }
        public async Task CancelAsync(int localDrivingLicenseApplicationId)
        {
            var application = await _localRepository.GetByIdAsync(localDrivingLicenseApplicationId);
            if (application is null) throw new Exception("Local driving license application not found.");
            if (application.Applications is null) throw new Exception("Application information not found.");
            if (application.Applications.ApplicationStatus != ApplicationStatus.New) throw new Exception("Only new applications can be cancelled.");

            application.Applications.ApplicationStatus = ApplicationStatus.Cancelled;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId)
        {
            return await _localRepository.HasActiveApplicationAsync(personId, licenseClassId);
        }
        public async Task<bool> IsThereAnActiveScheduledTest(int ldlaId, int testTypeID)
        {
            return await _testAppointmentRepository.IsThereAnActiveScheduledTest(ldlaId, testTypeID);
        }
        public async Task<bool> DoesPassTestType(int ldlaId, int testTypeID)
        {
            return await _localRepository.DoesPassTestType(ldlaId, testTypeID);
        }
    }
}