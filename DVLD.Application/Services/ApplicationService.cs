using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;


namespace DVLD.Application.Services
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

            var application = new Applications
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
    }
}