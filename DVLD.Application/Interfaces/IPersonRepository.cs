using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person?> GetByAuthUserIdAsync(int authUserId);
        Task<Person?> GetPersonByIdAsync(int PersonId);

        Task<Person> AddAsync(Person person);
        Task UpdateAsync(Person person);
    }
}