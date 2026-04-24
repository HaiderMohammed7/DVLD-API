using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;

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

        public async Task<PersonDto?> GetPersonByIdAsync(int personId)
        {
            var person = await _repo.GetPersonByIdAsync(personId);

            if (person == null)
                return null;

            return MapToDto(person);
        }

        public async Task<PersonDto> GetMyProfileAsync()
        {
            var authUserId = _currentUser.AuthUserId;

            var Person = await _repo.GetByAuthUserIdAsync(authUserId);

            if (Person == null)
                throw new Exception("Profile not found");

            return MapToDto(Person);
        }

        public async Task<PersonDto> CreatePersonAsync(CreatePersonDto request)
        {
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

            await _repo.AddAsync(person);

            return MapToDto(person);
        }

        public async Task<bool> UpdatePersonAsync(int personId, UpdatePersonDto request)
        {
            var person = await _repo.GetPersonByIdAsync(personId);

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

            await _repo.UpdateAsync(person);

            return true;
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