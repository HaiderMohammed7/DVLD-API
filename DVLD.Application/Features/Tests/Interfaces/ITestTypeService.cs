using DVLD.Application.Features.Tests.DTOs;

namespace DVLD.Application.Features.Tests.Interfaces
{
    public interface ITestTypeService
    {
        Task<IEnumerable<TestTypeListDto>> GetAllAsync();

        Task<GetTestTypeDto?> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateTestTypeDto dto);
    }
}