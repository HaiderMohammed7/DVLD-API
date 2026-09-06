using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

namespace DVLD.Application.Services
{
    public class LocalDrivingLicenseApplicationService : ILocalDrivingLicenseApplicationService
    {
        private readonly IApplicationService _applicationService;
        private readonly ILocalDrivingLicenseApplicationRepository _localRepository;
        private readonly ILicenseClassRepository _licenseClassRepository;
        private readonly IApplicationTypeRepository _applicationTypeRepository;

        public LocalDrivingLicenseApplicationService(IApplicationService applicationService, ILocalDrivingLicenseApplicationRepository localRepository, ILicenseClassRepository licenseClassRepository, IApplicationTypeRepository applicationTypeRepository)
        {
            _applicationService = applicationService;
            _localRepository = localRepository;
            _licenseClassRepository = licenseClassRepository;
            _applicationTypeRepository = applicationTypeRepository;
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
    }
}