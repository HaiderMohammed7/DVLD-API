using DVLD.Application.DTOs;
using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Features.Licenses.DTOs;
using DVLD.Application.Features.Licenses.Interfaces;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using DriverEntity = DVLD.Domain.Entities.Driver;
using LicenseEntity = DVLD.Domain.Entities.License;

namespace DVLD.Application.Features.Licenses.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly ILocalDrivingLicenseApplicationRepository _ldlaRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IUserService _userService;
        private readonly ILicenseClassRepository _licenseClassRepository;
        private readonly IApplicationService _applicationService;
        private readonly IApplicationTypeService _applicationTypeService;
        private readonly IDetainedLicenseRepository _detainedLicenseRepository;
        public LicenseService(ILicenseRepository licenseRepository, ILocalDrivingLicenseApplicationRepository ldlaRepository, IDriverRepository driverRepository, IUserService userService, ILicenseClassRepository licenseClassRepository, IApplicationService applicationService, IDetainedLicenseRepository detainedLicenseRepository, IApplicationTypeService applicationTypeService)
        {
            _licenseRepository = licenseRepository;
            _ldlaRepository = ldlaRepository;
            _driverRepository = driverRepository;
            _userService = userService;
            _licenseClassRepository = licenseClassRepository;
            _applicationService = applicationService;
            _detainedLicenseRepository = detainedLicenseRepository;
            _applicationTypeService = applicationTypeService;
        }

        public async Task<GetLicenseInfoDto?> GetForDetailsAsync(int licenseID)
        {
            var license = await _licenseRepository.GetForDetailsAsync(licenseID);

            if (license is null)
                return null;

            return new GetLicenseInfoDto
            {
                LicenseClassName = license.Classes.ClassName,

                FullName = license.Driver.Person.FirstName + " " +
                license.Driver.Person.SecondName + " " +
                license.Driver.Person.ThirdName + " " +
                license.Driver.Person.LastName,

                NationalNo = license.Driver.Person.NationalNo,

                DateOfBirth = license.Driver.Person.DateOfBirth,

                Gender = license.Driver.Person.Gendor.ToString(),

                DriverID = license.DriverID,

                DefaultValidityLength = license.Classes.DefaultValidityLength,
                ClassFees = license.Classes.ClassFees,

                IsActive = license.IsActive,

                IssueDate = license.IssueDate,

                ExpirationDate = license.ExpirationDate,

                IssueReason = ((IssueReasonEnum)license.IssueReason).ToString(),

                IsDetained = license.DetainedLicenses.Any(x => !x.IsReleased),

                Notes = license.Notes,

                ImagePath = license.Driver.Person.ImagePath
            };
        }

        public async Task<int> IssueDriverLicenseAsync(IssueDriverLicenseDto dto)
        {
            var ldla = await _ldlaRepository.GetByIdAsync(dto.LocalDrivingLicenseApplicationID);
            if (ldla == null) throw new Exception("Local Driving License Application not found.");

            var visionPassed = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID,(int)TestTypeEnum.Vision);
            var writtenPassed = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID, (int)TestTypeEnum.Written);
            var streetPassed = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID,(int)TestTypeEnum.Street);

            if (!visionPassed || !writtenPassed || !streetPassed) throw new Exception("Person should pass all tests first.");

            var user = await _userService.GetCurrentUserAsync();
            if (user == null || user.UserID == 0) throw new UnauthorizedAccessException("User is not found.");

            var personId = ldla.Applications!.ApplicantPersonID;

            var driver = await _driverRepository.GetByPersonIdAsync(personId);

            if (driver == null)
            {
                driver = new DriverEntity
                {
                    PersonID = personId,
                    CreatedByUserID = user.UserID,
                    CreatedDate = DateTime.Now
                };

                await _driverRepository.AddAsync(driver);
            }
            var driverId = driver.DriverID;

            var activeLicense = await _licenseRepository.GetActiveLicenseByDriverIdAndClassAsync( driver.DriverID,ldla.LicenseClassID);
            if (activeLicense != null) throw new Exception("This person already has an active license for this class");

            var licenseClass = await _licenseClassRepository.GetByIdAsync(ldla.LicenseClassID);
            if (licenseClass == null) throw new Exception("License class not found.");

            var issueDate = DateTime.Now;

            var license = new LicenseEntity
            {
                ApplicationID = ldla.Applications.ApplicationID,
                DriverID = driver.DriverID,
                LicenseClass = ldla.LicenseClassID,
                IssueDate = issueDate,
                ExpirationDate = issueDate.AddYears(licenseClass.DefaultValidityLength),
                Notes = dto.Notes ?? string.Empty,
                PaidFees = licenseClass.ClassFees,
                IsActive = true,
                IssueReason = 1,
                CreatedByUserID = user.UserID
            };

            await _licenseRepository.AddAsync(license);

            var updated = await _applicationService.UpdateStatusAsync(ldla.Applications.ApplicationID);
            if (!updated) throw new Exception("Failed to update application status.");

            return license.LicenseID;
        }

        public async Task<int> DetainLicenseAsync(DetainLicenseDto dto)
        {
            var license = await _licenseRepository.GetForDetailsAsync(dto.LicenseID);
            if (license == null)return -1;
            if (!license.IsActive)return -1;

            var activeDetain = await _detainedLicenseRepository.GetActiveDetainByLicenseIdAsync(dto.LicenseID);
            if (activeDetain != null) return -1;

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null) return -1;

            var detainedLicense = new DetainedLicense
            {
                LicenseID = dto.LicenseID,
                DetainDate = DateTime.UtcNow,
                FineFees = dto.FineFees,
                CreatedByUserID = currentUser.UserID,
                IsReleased = false
            };

            await _detainedLicenseRepository.AddAsync(detainedLicense);

            return detainedLicense.DetainID;
        }

        public async Task<int> ReleaseDetainedLicenseAsync(int licenseID)
        {
            var detainedLicense = await _detainedLicenseRepository.GetActiveDetainByLicenseIdAsync(licenseID);
            if (detainedLicense == null) return -1;

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("Current user not found.");

            var applicationType = await _applicationTypeService.GetByIdAsync((int)ApplicationTypeEnum.ReleaseDetainedLicense);
            if (applicationType == null) throw new Exception("Application type not found.");

            var applicationId = await _applicationService.CreateAsync(new CreateApplicationDto
                {
                    ApplicantPersonId = detainedLicense.License.Driver.PersonID,
                    ApplicationTypeId =(int)ApplicationTypeEnum.ReleaseDetainedLicense,
                    PaidFees = applicationType.Fees
                });

            var dto = new UpdateDetainLicenseDto()
            {
                DetainId = detainedLicense.DetainID,
                releaseDate = DateTime.UtcNow,
                ReleasedByUserId = currentUser.UserID,
                ReleaseApplicationId = applicationId,
            };

            await _detainedLicenseRepository.UpdateForReleaseAsync(dto);

            return applicationId;
        }

        public async Task<GetReleaseLicenseInfo?> ReleaseInfo(int licenseId)
        {
            var detainedLicense = await _detainedLicenseRepository.GetActiveDetainByLicenseIdAsync(licenseId);
            if (detainedLicense == null) return null;

            var applicationType = await _applicationTypeService.GetByIdAsync((int)ApplicationTypeEnum.ReleaseDetainedLicense);
            if (applicationType == null) throw new Exception("Application type not found.");

            return new GetReleaseLicenseInfo()
            {
                DetainId = detainedLicense.DetainID,
                DetainDate = detainedLicense.DetainDate,
                FineFees = detainedLicense.FineFees,
                ApplicationFees = applicationType.Fees
            };
        }

        public async Task<List<DetainedListDto>> GetDetainedList()
        {
            var lists = await _detainedLicenseRepository.GetDetainedList();

            return lists.Select(x => new DetainedListDto
            {
                DetainId = x.DetainID,
                DetinDate = x.DetainDate,
                ReleaseApplicationId = x.ReleaseApplicationID ?? 0,
                ReleaseDate = x.ReleaseDate,
                IsRelease = x.IsReleased,
                FineFees = x.FineFees,
                LicenseId = x.LicenseID,
                NationalNo = x.License.Driver.Person.NationalNo,
                FullName = x.License.Driver.Person.FirstName + " " + x.License.Driver.Person.SecondName
                + " " + x.License.Driver.Person.ThirdName + " " + x.License.Driver.Person.LastName,
            }).ToList();
        }
    }
}