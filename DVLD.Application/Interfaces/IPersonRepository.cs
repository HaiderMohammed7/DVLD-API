using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person?> GetByAuthUserIdAsync(int authUserId);
        Task<Person?> GetByIdAsync(int personId);
        Task<Person?> GetByNationalNoAsync(string nationalNo);
        Task<List<Person>> GetAllAsync();

        Task<Person> AddAsync(Person person);
        Task<bool> UpdateAsync(Person person);
        Task<bool> DeleteAsync(int personId);

        Task<bool> ExistsByNationalNoAsync(string nationalNo);
        Task<bool> ExistsByIdAsync(int personId); 
    }
}