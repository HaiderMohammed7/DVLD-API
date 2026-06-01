using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;

namespace DVLD.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly ICurrentUserService _currentUser;

        public PersonService(IPersonRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUser = currentUserService;
        }

        public async Task<PersonDto> GetMyProfileAsync()
        {
            var authUserId = _currentUser.AuthUserId;

            var Person = await _repo.GetByAuthUserIdAsync(authUserId);

            if (Person == null)
                throw new Exception("Profile not found");

            return MapToDto(Person);
        }
        public async Task<PersonDto?> GetByIdAsync(int personId)
        {
            var person = await _repo.GetByIdAsync(personId);

            if (person == null)
                return null;

            return MapToDto(person);
        }
        public async Task<PersonDto?> GetByNationalNoAsync(string nationalNo)
        {
            var person = await _repo.GetByNationalNoAsync(nationalNo);

            if (person == null) return null;

            return MapToDto(person);
        }
        public async Task<List<PersonDto>> GetAllAsync()
        {
            var people = await _repo.GetAllAsync();

            return people.Select(MapToDto).ToList();
        }

        public async Task<PersonDto> CreateAsync(CreatePersonDto request)
        {
            var nationalNoExists = await _repo.ExistsByNationalNoAsync(request.NationalNo);

            if (nationalNoExists) throw new Exception("National Number already exists");

            var person = new Person
            {
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                ThirdName = request.ThirdName,
                LastName = request.LastName,
                Email = request.Email,
                Address = request.Address,
                Phone = request.Phone,
                ImagePath = request.ImagePath,
                DateOfBirth = request.DateOfBirth,
                Gendor = request.Gendor,
                NationalNo = request.NationalNo,
                NationalityCountryID = request.NationalityCountryID,
            };

            var createdPerson = await _repo.AddAsync(person);

            return MapToDto(createdPerson);
        }
        public async Task<bool> UpdateAsync( int personId, UpdatePersonDto request)
        {
            var person = await _repo.GetByIdAsync(personId);

            if (person == null)
                return false;

            person.FirstName = request.FirstName;
            person.SecondName = request.SecondName;
            person.ThirdName = request.ThirdName;
            person.LastName = request.LastName;
            person.Email = request.Email;
            person.Address = request.Address;
            person.Phone = request.Phone;
            person.ImagePath = request.ImagePath;

            return await _repo.UpdateAsync();
        }
        public async Task<bool> DeleteAsync(int personId)
        {
            var exists = await _repo.ExistsByIdAsync(personId);

            if (!exists)
                return false;

            return await _repo.DeleteAsync(personId);
        }

        public async Task<bool> ExistsByIdAsync(int personId)
        {
            return await _repo.ExistsByIdAsync(personId);
        }
        public async Task<bool> ExistsByNationalNoAsync(string nationalNo)
        {
            return await _repo.ExistsByNationalNoAsync(nationalNo);
        }

        private static PersonDto MapToDto(Person p)
        {
            return new PersonDto
            {
                PersonID = p.PersonID,
                NationalNo = p.NationalNo,
                FullName = string.Join(" ",new[]{p.FirstName, p.SecondName,p.ThirdName,p.LastName}.Where(x => !string.IsNullOrWhiteSpace(x))),
                DateOfBirth = p.DateOfBirth,
                Gendor = p.Gendor,
                Address = p.Address,
                Phone = p.Phone,
                Email = p.Email,
                NationalityCountryID = p.NationalityCountryID,
                ImagePath = p.ImagePath
            };
        }
    }
}