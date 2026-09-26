using DVLD.Application.DTOs;
using DVLD.Application.Features.Driver.DTOs;

namespace DVLD.Application.Features.Driver.Interfaces
{
    public interface IDriverService
    {
        Task<List<GetDriverInternationalLicenseDto>> GetDriverInternationalLicensesAsync(int personId);
        Task<List<GetDriverLocalLicenseDto>> GetDriverLocalLicensesAsync(int personId);

        Task<List<DriverListDto>> GetAllAsync();
        Task<DriverInfoDto> GetDriverByIdAsync(int driverId);
    }
}