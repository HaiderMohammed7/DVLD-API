using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface ITestTypeRepository
    {
        Task<List<TestType>> GetAllAsync();

        Task<TestType?> GetByIdAsync(int id);

        Task UpdateAsync();
    }
}