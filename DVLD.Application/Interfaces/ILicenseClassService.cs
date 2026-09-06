using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces
{
    public interface ILicenseClassService
    {
        Task<List<LicenseClassDto>> GetAllAsync();
    }
}