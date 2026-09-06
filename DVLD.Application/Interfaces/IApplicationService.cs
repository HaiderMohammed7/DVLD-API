using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface IApplicationService
    {
        Task<int> CreateAsync(CreateApplicationDto dto);
    }
}