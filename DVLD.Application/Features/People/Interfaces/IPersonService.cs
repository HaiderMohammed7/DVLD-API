using DVLD.Application.Features.Common.DTOs;
using DVLD.Application.Features.People.DTOs;

namespace DVLD.Application.Features.People.Interfaces
{
    public interface IPersonService
    {
        Task<PersonDto?> GetMyProfileAsync();
        Task<PersonDto?> GetByIdAsync(int personId);
        Task<PersonDto?> GetByNationalNoAsync(string nationalNo);
        Task<List<PeopleListDto>> GetAllAsync();
        
        Task<int> CreateAsync(CreatePersonDto request);
        Task UpdateAsync(int personId, UpdatePersonDto request);
        Task DeleteAsync(int personId);

        Task<bool> ExistsByIdAsync(int personId);
        Task<bool> ExistsByNationalNoAsync(string nationalNo);
        Task<string?> GetCountryNameByIdAsync(int personId);
        Task<List<CountryDto>> GetCountries();
    }
}