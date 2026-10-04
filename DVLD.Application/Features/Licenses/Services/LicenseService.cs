using DVLD.Application.DTOs;
using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Features.Licenses.DTOs;
using DVLD.Application.Features.Licenses.Interfaces;
using DVLD.Application.Features.LocalDrivingLicenseApplications.DTOs;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using DriverEntity = DVLD.Domain.Entities.Driver;
using LicenseEntity = DVLD.Domain.Entities.License;
using ApplicationEntity = DVLD.Domain.Entities.Applications;

namespace DVLD.Application.Features.Licenses.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly ILocalDrivingLicenseApplicationRepository _ldlaRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILicenseClassRepository _licenseClassRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationTypeRepository _applicationTypeRepository;
        private readonly IDetainedLicenseRepository _detainedLicenseRepository;

        public LicenseService(IApplicationRepository applicationRepository, IApplicationTypeRepository applicationTypeRepository, ILicenseRepository licenseRepository, ILocalDrivingLicenseApplicationRepository ldlaRepository, IDriverRepository driverRepository, IUserService userService, ILicenseClassRepository licenseClassRepository, IDetainedLicenseRepository detainedLicenseRepository, IApplicationTypeService applicationTypeService, IUnitOfWork unitOfWork)
        {
            _licenseRepository = licenseRepository;
            _ldlaRepository = ldlaRepository;
            _driverRepository = driverRepository;
            _userService = userService;
            _licenseClassRepository = licenseClassRepository;
            _detainedLicenseRepository = detainedLicenseRepository;
            _unitOfWork = unitOfWork;
            _applicationRepository = applicationRepository;
            _applicationTypeRepository = applicationTypeRepository;
        }

        public async Task<GetLicenseInfoDto?> GetByIdAsync(int licenseID)
        {
            var license = await _licenseRepository.GetByIdAsync(licenseID);
            if (license is null) return null;

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

        public async Task<int> IssueAsync(IssueDriverLicenseDto dto)
        {
            var id = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var ldla = await _ldlaRepository.GetByIdAsync(dto.LocalDrivingLicenseApplicationID);
                if (ldla == null) throw new Exception("Local Driving License Application not found.");

                var visionPassed = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID, (int)TestTypeEnum.Vision);
                var writtenPassed = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID, (int)TestTypeEnum.Written);
                var streetPassed = await _ldlaRepository.DoesPassTestType(dto.LocalDrivingLicenseApplicationID, (int)TestTypeEnum.Street);

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
                else
                {
                    var activeLicense = await _licenseRepository.GetActiveLicenseByDriverIdAndClassAsync(driver.DriverID,ldla.LicenseClassID);
                    if (activeLicense != null) throw new Exception("This person already has an active license for this class.");
                }

                var licenseClass = await _licenseClassRepository.GetByIdAsync(ldla.LicenseClassID);
                if (licenseClass == null) throw new Exception("License class not found.");

                var issueDate = DateTime.Now;

                var license = new LicenseEntity
                {
                    ApplicationID = ldla.Applications.ApplicationID,
                    Driver = driver,
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

                var application = await _applicationRepository.GetByIdAsync(ldla.ApplicationID);
                if (application is null) throw new Exception("application not found.");

                application.ApplicationStatus = ApplicationStatus.Completed;
                application.LastStatusDate = DateTime.Now;

                return license.LicenseID;
            });

            return id;
        }

        public async Task<int> DetainAsync(DetainLicenseDto dto)
        {
            var license = await _licenseRepository.GetByIdAsync(dto.LicenseID);
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
            await _unitOfWork.SaveChangesAsync();

            return detainedLicense.DetainID;
        }
        public async Task<int> ReleaseAsync(int licenseID)
        {
            var id = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var detainedLicense = await _detainedLicenseRepository.GetActiveDetainByLicenseIdAsync(licenseID);
                if (detainedLicense == null) return -1;

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null) throw new Exception("Current user not found.");

                var applicationType = await _applicationTypeRepository.GetByIdAsync((int)ApplicationTypeEnum.ReleaseDetainedLicense);
                if (applicationType == null) throw new Exception("Application type not found.");

                var application = new ApplicationEntity
                {
                    ApplicantPersonID = detainedLicense.License.Driver.PersonID,
                    ApplicationTypeID = (int)ApplicationTypeEnum.ReleaseDetainedLicense,
                    PaidFees = applicationType.ApplicationFees,

                    ApplicationDate = DateTime.UtcNow,
                    LastStatusDate = DateTime.UtcNow,
                    ApplicationStatus = ApplicationStatus.New,
                    CreatedByUserID = currentUser.UserID
                };

                await _applicationRepository.AddAsync(application);

                detainedLicense.IsReleased = true;
                detainedLicense.ReleaseDate = DateTime.UtcNow;
                detainedLicense.ReleasedByUserID = currentUser.UserID;
                detainedLicense.ReleaseApplicationID = application.ApplicationID;

                return application.ApplicationID;
            });

            return id;
        }

        public async Task<ResponseRenewLicenseDto?> RenewAsync(int licenseId, string? notes)
        {
            var response = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var oldLicense = await _licenseRepository.GetByIdAsync(licenseId);
                if (oldLicense == null) return null;
                if (oldLicense.ExpirationDate.Date >= DateTime.Today) return null;

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null) return null;

                var applicationType = await _applicationTypeRepository.GetByIdAsync((int)ApplicationTypeEnum.RenewDrivingLicense);
                if (applicationType == null) return null;

                var application = new ApplicationEntity
                {
                    ApplicantPersonID = oldLicense.Driver!.PersonID,
                    ApplicationTypeID = (int)ApplicationTypeEnum.RenewDrivingLicense,
                    PaidFees = applicationType.ApplicationFees,

                    ApplicationDate = DateTime.UtcNow,
                    LastStatusDate = DateTime.UtcNow,
                    ApplicationStatus = ApplicationStatus.New,
                    CreatedByUserID = currentUser.UserID
                };

                await _applicationRepository.AddAsync(application);

                var newLicense = new License
                {
                    ApplicationID = application.ApplicationID,
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

                oldLicense.IsActive = false;

                return new ResponseRenewLicenseDto()
                {
                    NewLicenseId = newLicense.LicenseID,
                    NewApplicationId = application.ApplicationID,
                };
            });

            return response;         
        }
        public async Task<ResponseReplaceLicenseDto?> ReplaceAsync(ReplaceLicenseDto dto)
        {
            var response = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var oldLicense = await _licenseRepository.GetByIdAsync(dto.LicenseID);
                if (oldLicense == null) return null;
                if (!oldLicense.IsActive) return null;

                if (dto.IssueReason != IssueReasonEnum.ReplacementForDamaged && dto.IssueReason != IssueReasonEnum.ReplacementForLost) return null;

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null) return null;

                ApplicationTypeEnum applicationTypeEnum;

                if (dto.IssueReason == IssueReasonEnum.ReplacementForDamaged) applicationTypeEnum = ApplicationTypeEnum.ReplacementDamagedLicense;
                else applicationTypeEnum = ApplicationTypeEnum.ReplacementLostLicense;

                var applicationType = await _applicationTypeRepository.GetByIdAsync((int)applicationTypeEnum);
                if (applicationType == null) return null;

                var application = new ApplicationEntity
                {
                    ApplicantPersonID = oldLicense.Driver!.PersonID,
                    ApplicationTypeID = (int)applicationTypeEnum,
                    PaidFees = applicationType.ApplicationFees,

                    ApplicationDate = DateTime.UtcNow,
                    LastStatusDate = DateTime.UtcNow,
                    ApplicationStatus = ApplicationStatus.New,
                    CreatedByUserID = currentUser.UserID
                };

                await _applicationRepository.AddAsync(application);

                var newLicense = new License
                {
                    ApplicationID = application.ApplicationID,
                    DriverID = oldLicense.DriverID,
                    LicenseClass = oldLicense.LicenseClass,
                    IssueDate = DateTime.Today,
                    ExpirationDate = oldLicense.ExpirationDate,
                    Notes = oldLicense.Notes,
                    PaidFees = oldLicense.PaidFees,
                    IsActive = true,
                    IssueReason = (byte)dto.IssueReason,
                    CreatedByUserID = currentUser.UserID
                };

                await _licenseRepository.AddAsync(newLicense);

                oldLicense.IsActive = false;

                return new ResponseReplaceLicenseDto()
                {
                    NewLicenseId = newLicense.LicenseID,
                    NewApplicationId = application.ApplicationID,
                };
            });

            return response;
        }

        public async Task<GetReleaseLicenseInfo?> ReleaseInfo(int licenseId)
        {
            var detainedLicense = await _detainedLicenseRepository.GetActiveDetainByLicenseIdAsync(licenseId);
            if (detainedLicense == null) return null;

            var applicationType = await _applicationTypeRepository.GetByIdAsync((int)ApplicationTypeEnum.ReleaseDetainedLicense);
            if (applicationType == null) throw new Exception("Application type not found.");

            return new GetReleaseLicenseInfo()
            {
                DetainId = detainedLicense.DetainID,
                DetainDate = detainedLicense.DetainDate,
                FineFees = detainedLicense.FineFees,
                ApplicationFees = applicationType.ApplicationFees
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