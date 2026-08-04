using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;

namespace DVLD.Application.Services
{
    public class ApplicationTypeService : IApplicationTypeService
    {
        private readonly IApplicationTypeRepository _repository;

        public ApplicationTypeService(IApplicationTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ApplicationTypeListDto>> GetAllAsync()
        {
            var applicationTypes = await _repository.GetAllAsync();

            return applicationTypes.Select(x => new ApplicationTypeListDto
            {
                Id = x.ApplicationTypeID,
                Title = x.ApplicationTypeTitle,
                Fees = x.ApplicationFees
            });
        }

        public async Task<GetApplicationTypeDto?> GetByIdAsync(int id)
        {
            var applicationType = await _repository.GetByIdAsync(id);

            if (applicationType == null)
                return null;

            return new GetApplicationTypeDto
            {
                Id = applicationType.ApplicationTypeID,
                Title = applicationType.ApplicationTypeTitle,
                Fees = applicationType.ApplicationFees
            };
        }

        public async Task UpdateAsync(int id, UpdateApplicationTypeDto dto)
        {
            var applicationType = await _repository.GetByIdAsync(id);

            if (applicationType == null)
                throw new Exception($"Application Type with ID = {id} not found.");

            applicationType.ApplicationTypeTitle = dto.Title;
            applicationType.ApplicationFees = dto.Fees;

            await _repository.UpdateAsync();
        }
    }
}