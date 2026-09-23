using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Features.Users.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Enums;
using ApplicationEntity = DVLD.Domain.Entities.Applications;

namespace DVLD.Application.Features.Applications.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserService _userService;

        public ApplicationService(IApplicationRepository applicationRepository,IUserService userService)
        {
            _applicationRepository = applicationRepository;
            _userService = userService;
        }

        public async Task<int> CreateAsync(CreateApplicationDto dto)
        {
            var currentUser = await _userService.GetCurrentUserAsync();

            if (currentUser == null)
                throw new Exception("Current user not found.");

            var application = new ApplicationEntity
            {
                ApplicantPersonID = dto.ApplicantPersonId,
                ApplicationTypeID = dto.ApplicationTypeId,
                PaidFees = dto.PaidFees,
                
                ApplicationDate = DateTime.UtcNow,
                LastStatusDate = DateTime.UtcNow,
                ApplicationStatus = ApplicationStatus.New,
                CreatedByUserID = currentUser.UserID
            };

            await _applicationRepository.AddAsync(application);

            return application.ApplicationID;
        }

        public async Task<GetApplicationInfoDto?> GetForDetailsAsync(int applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);

            if (application is null)
                return null;

            return new GetApplicationInfoDto
            {
                ApplicationID = application.ApplicationID,

                Status = application.ApplicationStatus.ToString(),

                PaidFees = application.PaidFees,

                ApplicationType = application.ApplicationType.ApplicationTypeTitle,

                ApplicantPersonID = application.ApplicantPersonID,

                ApplicantName = application.Person.FirstName + " " +
                application.Person.SecondName + " " +
                application.Person.ThirdName + " " +
                application.Person.LastName,

                ApplicationDate = application.ApplicationDate,

                StatusDate = application.LastStatusDate,

                CreatedByUserID = application.CreatedByUserID
            };
        }

        public async Task<bool> UpdateStatusAsync(int applicationId)
        {
            return await _applicationRepository.UpdateStatusAsync(applicationId);
        }
    }
}