using DVLD.Application.DTOs;
using DVLD.Application.Features.InternationalLicense.Interfaces;
using DVLD.Application.Interfaces.Repositories;

namespace DVLD.Application.Features.InternationalLicense.Services
{
    public class InternationalLicenseService : IInternationalLicenseService
    {
        private readonly IInternationalLicenseRepository _repository;
        public InternationalLicenseService(IInternationalLicenseRepository internationalLicense)
        {
            _repository = internationalLicense;
        }

        public async Task<GetInternationalLicenseInfoDto?> GetInternationalLicenseInfoAsync(int internationalLicenseId)
        {
            return await _repository.GetInternationalLicenseInfoAsync(internationalLicenseId);
        }
    }
}