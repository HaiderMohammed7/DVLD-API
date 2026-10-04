using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Interfaces.Repositories;

namespace DVLD.Application.Features.Applications.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;;
        }

        public async Task<GetApplicationInfoDto?> GetByIdAsync(int applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application is null) return null;

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
    }
}