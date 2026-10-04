using DVLD.Application.Features.Common.DTOs;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ICountryRepositroy
    {
        Task<bool> ExistsAsync(int CountryId);
        Task<List<CountryDto>> GetCountries();
    }
}