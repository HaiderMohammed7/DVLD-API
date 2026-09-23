using DVLD.Application.Features.Applications.DTOs;

namespace DVLD.Application.Features.Applications.Interfaces
{
    public interface IApplicationService
    {
        Task<int> CreateAsync(CreateApplicationDto dto);
        Task<GetApplicationInfoDto?> GetForDetailsAsync(int applicationId);
        Task<bool> UpdateStatusAsync(int applicationId);
    }
}