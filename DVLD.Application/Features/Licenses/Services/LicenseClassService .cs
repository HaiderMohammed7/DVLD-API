using DVLD.Application.Features.Licenses.DTOs;
using DVLD.Application.Features.Licenses.Interfaces;
using DVLD.Application.Interfaces.Repositories;

namespace DVLD.Application.Features.Licenses.Services
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