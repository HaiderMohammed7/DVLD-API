using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface ITestTypeService
    {
        Task<IEnumerable<TestTypeListDto>> GetAllAsync();

        Task<GetTestTypeDto?> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateTestTypeDto dto);
    }
}