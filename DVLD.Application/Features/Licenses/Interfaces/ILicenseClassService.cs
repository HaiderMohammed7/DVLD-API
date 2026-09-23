using DVLD.Application.Features.Licenses.DTOs;

namespace DVLD.Application.Features.Licenses.Interfaces
{
    public interface ILicenseClassService
    {
        Task<List<LicenseClassDto>> GetAllAsync();
    }
}