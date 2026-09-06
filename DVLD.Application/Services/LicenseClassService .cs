using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;

namespace DVLD.Application.Services
{
    public class LicenseClassService : ILicenseClassService
    {
        private readonly ILicenseClassRepository _licenseClassRepository;

        public LicenseClassService(ILicenseClassRepository licenseClassRepository)
        {
            _licenseClassRepository = licenseClassRepository;
        }

        public async Task<List<LicenseClassDto>> GetAllAsync()
        {
            var licenseClasses = await _licenseClassRepository.GetAllAsync();

            return licenseClasses.Select(c => new LicenseClassDto
            {
                LicenseClassId = c.LicenseClassID,
                ClassName = c.ClassName
            }).ToList();
        }
    }
}