using DVLD.Application.Features.Applications.DTOs;

namespace DVLD.Application.Features.Applications.Interfaces
{
    public interface IApplicationTypeService
    {
        Task<IEnumerable<ApplicationTypeListDto>> GetAllAsync();

        Task<GetApplicationTypeDto?> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateApplicationTypeDto dto);
    }
}