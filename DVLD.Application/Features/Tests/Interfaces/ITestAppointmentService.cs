using DVLD.Application.Features.Tests.DTOs;

namespace DVLD.Application.Features.Tests.Interfaces
{
    public interface ITestAppointmentService
    {
        Task<TestAppointmentDTO?> GetByIdAsync(int id);
        Task<List<AppointmentsListDto>> GetAllAsync();

        Task<ScheduleTestResultDto> ScheduleTestAsync(ScheduleTestDto dto);
        Task<bool> UpdateAppointmentDateAsync(UpdateTestAppointmentDto dto);
        Task<GetScheduleTestInfoDto?> GetScheduleTestInfoAsync(int localDrivingLicenseApplicationID, int testTypeID);

        Task<GetScheduledTestInfoDto?> GetScheduledTestInfoAsync(int testAppointmentID);
    }
}