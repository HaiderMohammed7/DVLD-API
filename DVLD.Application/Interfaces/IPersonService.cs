using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface IPersonService
    {
        Task<PersonDto> GetMyProfileAsync();
        Task<PersonDto?> GetByIdAsync(int personId);
        Task<PersonDto?> GetByNationalNoAsync(string nationalNo);
        Task<List<PeopleListDto?>> GetAllAsync();
        
        Task<PersonDto> CreateAsync(CreatePersonDto request);
        Task<bool> UpdateAsync(int personId, UpdatePersonDto request);
        Task<bool> DeleteAsync(int personId);

        Task<bool> ExistsByIdAsync(int personId);
        Task<bool> ExistsByNationalNoAsync(string nationalNo);
        Task<string?> GetCountryNameByIdAsync(int personId);
    }
}