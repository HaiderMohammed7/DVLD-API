using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;

namespace DVLD.Application.Services
{
    public class TestTypeService : ITestTypeService
    {
        private readonly ITestTypeRepository _repository;

        public TestTypeService(ITestTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TestTypeListDto>> GetAllAsync()
        {
            var testType = await _repository.GetAllAsync();

            return testType.Select(x => new TestTypeListDto
            {
                Id = x.TestTypeID,
                Title = x.TestTypeTitle,
                Description = x.TestTypeDescription,
                Fees = x.TestTypeFees
            });
        }

        public async Task<GetTestTypeDto?> GetByIdAsync(int id)
        {
            var testType = await _repository.GetByIdAsync(id);

            if (testType == null)
                return null;

            return new GetTestTypeDto
            {
                Id = testType.TestTypeID,
                Title = testType.TestTypeTitle,
                Description= testType.TestTypeDescription,
                Fees = testType.TestTypeFees
            };
        }

        public async Task UpdateAsync(int id, UpdateTestTypeDto dto)
        {
            var testType = await _repository.GetByIdAsync(id);

            if (testType == null)
                throw new Exception($"Test Type with ID = {id} not found.");

            testType.TestTypeTitle = dto.Title;
            testType.TestTypeDescription = dto.Description;
            testType.TestTypeFees = dto.Fees;

            await _repository.UpdateAsync();
        }
    }
}