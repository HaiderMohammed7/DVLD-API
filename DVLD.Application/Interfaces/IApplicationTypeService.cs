using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface IApplicationTypeService
    {
        Task<IEnumerable<ApplicationTypeListDto>> GetAllAsync();

        Task<GetApplicationTypeDto?> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateApplicationTypeDto dto);
    }
}