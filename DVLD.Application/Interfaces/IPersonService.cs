using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface IPersonService
    {
        Task<PersonDto?> GetPersonByIdAsync(int personId);
        Task<PersonDto> GetMyProfileAsync();
        Task<PersonDto> CreatePersonAsync(CreatePersonDto request);
        Task<bool> UpdatePersonAsync(int personId, UpdatePersonDto request);
    }
}