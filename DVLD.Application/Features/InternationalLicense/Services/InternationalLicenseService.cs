using DVLD.Application.DTOs;
using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Features.InternationalLicense.Interfaces;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Enums;
using InternationalEntity = DVLD.Domain.Entities.InternationalLicense;

namespace DVLD.Application.Features.InternationalLicense.Services
{
    public class InternationalLicenseService : IInternationalLicenseService
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicationTypeService _applicationTypeService;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IInternationalLicenseRepository _repository;
        private readonly IUserService _userService;
        public InternationalLicenseService(IInternationalLicenseRepository internationalLicense, IApplicationService applicationService, IApplicationTypeService applicationTypeService, ILicenseRepository licenseRepository, IUserService userService)
        {
            _applicationService = applicationService;
            _applicationTypeService = applicationTypeService;
            _licenseRepository = licenseRepository;
            _repository = internationalLicense;
            _userService = userService;
        }

        public async Task<GetInternationalLicenseInfoDto?> GetInternationalLicenseInfoAsync(int internationalLicenseId)
        {
            return await _repository.GetInternationalLicenseInfoAsync(internationalLicenseId);
        }

        public async Task<int> IssueInternationalLicenseAsync(int licenseId)
        {
            var license = await _licenseRepository.GetValidLicenseForInternationalAsync(licenseId);
            if (license == null) throw new Exception("Local license is not valid for issuing an international license.");

            var existingInternationalLicense = await _repository.GetActiveByDriverIdAsync(license.DriverID);
            if (existingInternationalLicense != null) throw new Exception("This driver already has an active international license.");

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("Current user not found.");

            var applicationType = await _applicationTypeService.GetByIdAsync((int)ApplicationTypeEnum.NewInternationalLicense);
            if (applicationType == null) throw new Exception("Application type not found.");

            var applicationId = await _applicationService.CreateAsync(new CreateApplicationDto
                {
                    ApplicantPersonId = license.Driver!.PersonID,
                    ApplicationTypeId = applicationType.Id,
                    PaidFees = applicationType.Fees
                });

            var issueDate = DateTime.UtcNow;

            var internationalLicense = new InternationalEntity
            {
                ApplicationID = applicationId,
                DriverID = license.DriverID,
                IssuedUsingLocalLicenseID = license.LicenseID,
                IssueDate = issueDate,
                ExpirationDate = issueDate.AddYears(1),
                IsActive = true,
                CreatedByUserID = currentUser.UserID
            };

            await _repository.AddAsync(internationalLicense);
            return internationalLicense.InternationalLicenseID;
        }

        public async Task<List<ListInternationalLicenseApplicationDto>> GetAllAsync()
        {
            var intLApp = await _repository.GetAllAsync();

            return intLApp.Select(x => new ListInternationalLicenseApplicationDto
            {
                InternationalLicenseID = x.InternationalLicenseID,
                ApplicationID = x.ApplicationID,
                DriverID= x.DriverID,
                IssuedUsingLocalLicenseID = x.IssuedUsingLocalLicenseID,
                IssueDate= x.IssueDate,
                ExpirationDate= x.ExpirationDate,
                IsActive = x.IsActive,
            }).ToList();
        }
    }
}