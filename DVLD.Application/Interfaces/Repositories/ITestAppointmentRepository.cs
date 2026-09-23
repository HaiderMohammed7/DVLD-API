using DVLD.Application.Features.Tests.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ITestAppointmentRepository
    {
        Task AddAsync(TestAppointment testAppointment);
        Task<bool> UpdateAppointmentDateAsync(int testAppointmentID, DateTime appointmentDate);

        Task<TestAppointment?> GetByIdAsync(int id);
        Task<List<TestAppointment>> GetAllAsync();

        Task<bool> HasTestAppointmentsAsync(int ldlaID);
        Task<bool> IsThereAnActiveScheduledTest(int localDrivingLicenseApplicationID, int testTypeID);

        Task<List<TestAppointment>> GetByLDLAAndTestTypeAsync( int localDrivingLicenseApplicationID, int testTypeID);

        Task<bool> LockAppointmentAsync(int appointmentId);

        Task<GetScheduledTestInfoDto?> GetScheduledTestInfoAsync(int testAppointmentID);
    }
}