using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface ICountryRepositroy
    {
        Task<bool> ExistsAsync(int CountryId);
        Task<List<CountryDto>> GetCountryNameAsync();
    }
}