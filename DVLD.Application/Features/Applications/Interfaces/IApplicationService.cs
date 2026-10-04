using DVLD.Application.Features.Applications.DTOs;

namespace DVLD.Application.Features.Applications.Interfaces
{
    public interface IApplicationService
    {
        Task<GetApplicationInfoDto?> GetByIdAsync(int applicationId);
    }
}