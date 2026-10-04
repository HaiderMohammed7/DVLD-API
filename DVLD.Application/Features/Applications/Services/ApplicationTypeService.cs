using DVLD.Application.Features.Applications.DTOs;
using DVLD.Application.Features.Applications.Interfaces;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Application.Interfaces.UintOfWork;

namespace DVLD.Application.Features.Applications.Services
{
    public class ApplicationTypeService : IApplicationTypeService
    {
        private readonly IApplicationTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationTypeService(IApplicationTypeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetApplicationTypeDto?> GetByIdAsync(int id)
        {
            var applicationType = await _repository.GetByIdAsync(id);
            if (applicationType == null) return null;

            return new GetApplicationTypeDto
            {
                Id = applicationType.ApplicationTypeID,
                Title = applicationType.ApplicationTypeTitle,
                Fees = applicationType.ApplicationFees
            };
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

        public async Task UpdateAsync(int id, UpdateApplicationTypeDto dto)
        {
            var applicationType = await _repository.GetByIdAsync(id);
            if (applicationType == null) throw new Exception($"Application Type with ID = {id} not found.");

            applicationType.ApplicationTypeTitle = dto.Title;
            applicationType.ApplicationFees = dto.Fees;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}