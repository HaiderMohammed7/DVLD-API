using DVLD.Application.DTOs;

namespace DVLD.Application.Features.Driver.Interfaces
{
    public interface IDriverService
    {
        Task<List<GetDriverInternationalLicenseDto>> GetDriverInternationalLicensesAsync(int personId);
        Task<List<GetDriverLocalLicenseDto>> GetDriverLocalLicensesAsync(int personId);

        Task<List<DriverListDto>> GetAllAsync();
    }
}