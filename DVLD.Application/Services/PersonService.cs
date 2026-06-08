using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;
using DVLD.Domain.Entities;

namespace DVLD.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly ICountryRepositroy _country;

        public PersonService(IPersonRepository repo, ICurrentUserService currentUserService, ICountryRepositroy country)
        {
            _repo = repo;
            _currentUser = currentUserService;
            _country = country;
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
        public async Task<List<PeopleListDto?>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<PersonDto> CreateAsync(CreatePersonDto request)
        {
            await ValidateCreateRequestAsync(request);

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

            await ValidateUpdateRequestAsync(personId, request);

            person.NationalNo = request.NationalNo;
            person.FirstName = request.FirstName;
            person.SecondName = request.SecondName;
            person.ThirdName = request.ThirdName;
            person.LastName = request.LastName;
            person.Email = request.Email;
            person.Address = request.Address;
            person.Phone = request.Phone;
            person.ImagePath = request.ImagePath;
            person.DateOfBirth = request.DateOfBirth;
            person.Gendor = request.Gendor;
            person.NationalityCountryID = request.NationalityCountryID;

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
                FirstName = p.FirstName,
                SecondName = p.SecondName,
                ThirdName = p.ThirdName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Gendor = p.Gendor,
                Address = p.Address,
                Phone = p.Phone,
                Email = p.Email,
                NationalityCountryID = p.NationalityCountryID,
                ImagePath = p.ImagePath
            };
        }

        private async Task ValidateCreateRequestAsync(CreatePersonDto request)
        {
            if (string.IsNullOrWhiteSpace(request.NationalNo))
                throw new Exception("National Number is required");

            var nationalNoExists = await _repo.ExistsByNationalNoAsync(request.NationalNo);
            if (nationalNoExists) throw new Exception("National Number already exists");


            if (request.NationalityCountryID <= 0)
                throw new Exception("Nationality is required");

            if (!await _country.ExistsAsync(request.NationalityCountryID))
                throw new Exception("Invalid country");

            if (request.DateOfBirth >= DateTime.Today)
                throw new Exception("Invalid date of birth");

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new Exception("First Name is required");

            if (string.IsNullOrWhiteSpace(request.SecondName))
                throw new Exception("Second Name is required");

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new Exception("Last Name is required");

            if (string.IsNullOrWhiteSpace(request.Phone))
                throw new Exception("Phone is required");

            if (string.IsNullOrWhiteSpace(request.Address))
                throw new Exception("Address is required");
        }

        private async Task ValidateUpdateRequestAsync(int personId,UpdatePersonDto request)
        {
            if (string.IsNullOrWhiteSpace(request.NationalNo))
                throw new Exception("National Number is required");

            var personWithSameNationalNo = await _repo.GetByNationalNoAsync(request.NationalNo);

            if (personWithSameNationalNo != null && personWithSameNationalNo.PersonID != personId)
            {
                throw new Exception("National Number already exists");
            }

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new Exception("First Name is required");

            if (string.IsNullOrWhiteSpace(request.SecondName))
                throw new Exception("Second Name is required");

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new Exception("Last Name is required");

            if (string.IsNullOrWhiteSpace(request.Phone))
                throw new Exception("Phone is required");

            if (string.IsNullOrWhiteSpace(request.Address))
                throw new Exception("Address is required");

            if (request.NationalityCountryID <= 0)
                throw new Exception("Nationality is required");

            if (!await _country.ExistsAsync(request.NationalityCountryID))
                throw new Exception("Invalid country");

            if (request.DateOfBirth >= DateTime.Today)
                throw new Exception("Invalid date of birth");
        }

        public async Task<string?> GetCountryNameByIdAsync(int personId)
        {
            return await _repo.GetCountryNameByIdAsync(personId);
        }
    }
}