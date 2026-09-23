using DVLD.Application.Features.Tests.DTOs;

namespace DVLD.Application.Features.Tests.Interfaces
{
    public interface ITestService
    {
        Task ConductTestAsync(ConductTestDto dto);
        Task<GetTestInfoDto> GetByIdAsync(int testId);
    }
}