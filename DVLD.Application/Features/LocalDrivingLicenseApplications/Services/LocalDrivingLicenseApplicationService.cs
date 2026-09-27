using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;
using DVLD.Application.Features.LocalDrivingLicenseApplications.Interfaces;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Features.LocalDrivingLicenseApplications.Services
{
    public class LocalDrivingLicenseApplicationService : ILocalDrivingLicenseApplicationService
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILocalDrivingLicenseApplicationRepository _localRepository;
        private readonly ILicenseClassRepository _licenseClassRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IApplicationTypeRepository _applicationTypeRepository;
        private readonly ITestAppointmentRepository _testAppointmentRepository;
        private readonly IUserService _userService;

        public LocalDrivingLicenseApplicationService(IApplicationService applicationService, IApplicationRepository applicationRepository, ILocalDrivingLicenseApplicationRepository localRepository, ILicenseClassRepository licenseClassRepository, IApplicationTypeRepository applicationTypeRepository, ITestAppointmentRepository testAppointmentRepository, ILicenseRepository licenseRepository, IUserService userService)
        {
            _applicationService = applicationService;
            _applicationRepository = applicationRepository;
            _localRepository = localRepository;
            _licenseClassRepository = licenseClassRepository;
            _applicationTypeRepository = applicationTypeRepository;
            _testAppointmentRepository = testAppointmentRepository;
            _licenseRepository = licenseRepository;
            _userService = userService;
        }

        public async Task<int> AddAsync(CreateLocalDrivingLicenseApplicationDto dto)
        {
            var licenseClass = await _licenseClassRepository.GetByIdAsync(dto.LicenseClassId);

            if (licenseClass is null)
                throw new Exception("License class not found.");

            bool hasActive = await _localRepository.HasActiveApplicationAsync(dto.PersonId, dto.LicenseClassId);

            if (hasActive)
                throw new Exception("Person already has an active application for this license class.");

            var applicationType = await _applicationTypeRepository.GetByIdAsync((int)ApplicationTypeEnum.NewLocalDrivingLicense);

            int applicationId = await _applicationService.CreateAsync(new CreateApplicationDto
            {
                ApplicantPersonId = dto.PersonId,
                ApplicationTypeId = (int)ApplicationTypeEnum.NewLocalDrivingLicense,
                PaidFees = applicationType.ApplicationFees
            });

            var application = new LocalDrivingLicenseApplication
            {
                ApplicationID = applicationId,
                LicenseClassID = dto.LicenseClassId
            };

            await _localRepository.AddAsync(application);

            return application.LocalDrivingLicenseApplicationID;
        }

        public async Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId)
        {
            return await _localRepository.HasActiveApplicationAsync(personId, licenseClassId);
        }

        public async Task UpdateAsync(int localDrivingLicenseApplicationId, UpdateLocalDrivingLicenseApplicationDto dto)
        {
            var application = await _localRepository.GetByIdAsync(localDrivingLicenseApplicationId);

            if (application is null)
                throw new Exception("Local driving license application not found.");

            var licenseClass = await _licenseClassRepository.GetByIdAsync(dto.LicenseClassId);

            if (licenseClass is null)
                throw new Exception("License class not found.");

            int personId = application.Applications.ApplicantPersonID;

            bool hasActive = await _localRepository.HasActiveApplicationAsync(personId, dto.LicenseClassId,localDrivingLicenseApplicationId);

            if (hasActive)
                throw new Exception("Person already has an active application for this license class.");

            application.LicenseClassID = dto.LicenseClassId;

            await _localRepository.UpdateAsync();
        }

        public async Task<GetLocalDrivingLicenseApplicationDto?> GetByIdAsync(int id)
        {
            var application = await _localRepository.GetByIdAsync(id);

            if (application is null)
                return null;

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

        public async Task CancelAsync(int localDrivingLicenseApplicationId)
        {
            var application = await _localRepository.GetByIdAsync(localDrivingLicenseApplicationId);

            if (application is null)
                throw new Exception("Local driving license application not found.");

            if (application.Applications is null)
                throw new Exception("Application information not found.");

            if (application.Applications.ApplicationStatus != ApplicationStatus.New)
                throw new Exception("Only new applications can be cancelled.");

            application.Applications.ApplicationStatus = ApplicationStatus.Cancelled;

            await _localRepository.UpdateAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var ldla = await _localRepository.GetByIdAsync(id);

            if (ldla is null)
                throw new Exception("Application not found.");

            if (ldla.Applications.ApplicationStatus != ApplicationStatus.New)
                throw new Exception("Only new applications can be deleted.");

            bool hasAppointments = await _testAppointmentRepository.HasTestAppointmentsAsync(id);

            if (hasAppointments)
                throw new Exception("Cannot delete an application that has test appointments.");

            await _localRepository.DeleteAsync(id);

            await _applicationRepository.DeleteAsync(ldla.ApplicationID);
        }

        public async Task<GetLocalDrivingLicenseApplicationInfoDto?> GetForDetailsAsync(int id)
        {
            var application = await _localRepository.GetForDetailsAsync(id);

            if (application is null)
                return null;

            return new GetLocalDrivingLicenseApplicationInfoDto
            {
                ApplicationID = application.ApplicationID,

                LicenseID = application.Applications.Licenses.FirstOrDefault(x => x.IsActive)?.LicenseID,

                LicenseClassName = application.LicenseClass.ClassName,

                PassedTests = application.TestAppointments.SelectMany(x => x.Tests) .Count(x => x.TestResult)
            };
        }

        public async Task<bool> IsThereAnActiveScheduledTest(int ldlaId, int testTypeID)
        {
            return await _testAppointmentRepository.IsThereAnActiveScheduledTest(ldlaId, testTypeID);
        }

        public async Task<bool> DoesPassTestType(int ldlaId, int testTypeID)
        {
            return await _localRepository.DoesPassTestType(ldlaId, testTypeID);
        }

        public async Task<ResponseRenewLicenseDto?> RenewLocalDrivingLicenseAsync(int licenseId, string? notes)
        {
            var oldLicense = await _licenseRepository.GetForDetailsAsync(licenseId);
            if (oldLicense == null) return null;
            if (oldLicense.ExpirationDate.Date >= DateTime.Today) return null;

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null) return null;

            var applicationType = await _applicationTypeRepository.GetByIdAsync((int)ApplicationTypeEnum.RenewDrivingLicense);
            if (applicationType == null) return null;

            var applicationId = await _applicationService.CreateAsync(new CreateApplicationDto
                {
                    ApplicantPersonId = oldLicense.Driver!.PersonID,
                    ApplicationTypeId = (int)ApplicationTypeEnum.RenewDrivingLicense,
                    PaidFees = applicationType.ApplicationFees
                });  

            var newLicense = new License
            {
                ApplicationID = applicationId,
                DriverID = oldLicense.DriverID,
                LicenseClass = oldLicense.LicenseClass,
                IssueDate = DateTime.Today,
                ExpirationDate = DateTime.Today.AddYears(oldLicense.Classes!.DefaultValidityLength),
                Notes = notes ?? string.Empty,
                PaidFees = oldLicense.Classes.ClassFees,
                IsActive = true,
                IssueReason = (byte)IssueReasonEnum.Renew,
                CreatedByUserID = currentUser.UserID,
            };

            await _licenseRepository.AddAsync(newLicense);

            var deactivated = await _licenseRepository.DeactivateAsync(oldLicense.LicenseID);
            if (!deactivated) return null;

            return new ResponseRenewLicenseDto()
            {
                NewLicenseId = newLicense.LicenseID,
                NewApplicationId = applicationId,
            };
        }
    }
}