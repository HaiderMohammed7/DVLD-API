using DVLD.Application.Features.Tests.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ITestAppointmentRepository
    {
        Task<TestAppointment?> GetByIdAsync(int id);
        Task<List<TestAppointment>> GetAllAsync();
        Task<List<TestAppointment>> GetByLDLAAndTestTypeAsync(int localDrivingLicenseApplicationID, int testTypeID);
        Task<GetScheduledTestInfoDto?> GetScheduledTestInfoAsync(int testAppointmentID);

        Task AddAsync(TestAppointment testAppointment);
     
        Task<bool> HasTestAppointmentsAsync(int ldlaID);
        Task<bool> IsThereAnActiveScheduledTest(int localDrivingLicenseApplicationID, int testTypeID);     
    }
}