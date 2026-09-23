using DVLD.Application.Features.People.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        Task<Person?> GetByAuthUserIdAsync(int authUserId);
        Task<Person?> GetByIdAsync(int personId);
        Task<Person?> GetByNationalNoAsync(string nationalNo);
        Task<List<PeopleListDto>> GetAllAsync();

        Task<Person> AddAsync(Person person);
        Task<bool> UpdateAsync();
        Task<bool> DeleteAsync(int personId);

        Task<bool> ExistsByNationalNoAsync(string nationalNo);
        Task<bool> ExistsByIdAsync(int personId);
        Task<string?> GetCountryNameByIdAsync(int personId);
    }
}