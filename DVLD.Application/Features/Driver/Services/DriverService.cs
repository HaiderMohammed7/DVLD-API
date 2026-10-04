using DVLD.Application.DTOs;
using DVLD.Application.Features.Driver.DTOs;
using DVLD.Application.Features.Driver.Interfaces;
using DVLD.Application.Interfaces.Repositories;

namespace DVLD.Application.Features.Driver.Services
{
    public class DriverService : IDriverService
    {
        private readonly IInternationalLicenseRepository _international;
        public readonly ILicenseRepository _LicenseRepository;
        private readonly IDriverRepository _DriverRepository;
        public DriverService(IInternationalLicenseRepository international, ILicenseRepository licenseRepository, IDriverRepository driverRepository)
        {
            _international = international;
            _LicenseRepository = licenseRepository;
            _DriverRepository = driverRepository;
        }

        public async Task<List<GetDriverInternationalLicenseDto>> GetDriverInternationalLicensesAsync(int personId)
        {
            return await _international.GetDriverInternationalLicensesAsync(personId);
        }
        public async Task<DriverInfoDto> GetDriverByIdAsync(int driverId)
        {
            var driver = await _DriverRepository.GetByIdAsync(driverId);
            if (driver == null) throw new KeyNotFoundException($"Driver with ID {driverId} not found.");

            return new DriverInfoDto()
            {
                PersonId = driver.PersonID,
                CreatedUserId = driver.CreatedByUserID,
                CreatedDate = driver.CreatedDate,
            };
        }
        public async Task<List<GetDriverLocalLicenseDto>> GetDriverLocalLicensesAsync(int personId)
        {
            return await _LicenseRepository.GetDriverLocalLicensesAsync(personId);
        }
        public async Task<List<DriverListDto>> GetAllAsync()
        {
            return await _DriverRepository.GetAllDrivers();
        }    
    }
}