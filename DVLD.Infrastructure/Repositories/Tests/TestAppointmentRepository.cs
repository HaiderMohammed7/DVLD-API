using DVLD.Application.Features.Tests.DTOs;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.Tests
{
    public class TestAppointmentRepository : ITestAppointmentRepository
    {
        private readonly DVLDDbContext _context;

        public TestAppointmentRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TestAppointment testAppointment)
        {
            await _context.TestAppointments.AddAsync(testAppointment);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateAppointmentDateAsync(int testAppointmentID, DateTime appointmentDate)
        {
            var appointment = await _context.TestAppointments.FirstOrDefaultAsync(x => x.TestAppointmentID == testAppointmentID);
            if (appointment == null) return false;

            appointment.AppointmentDate = appointmentDate;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TestAppointment?> GetByIdAsync(int id)
        {
            return await _context.TestAppointments.Include(x => x.Applications)
                .FirstOrDefaultAsync(x => x.TestAppointmentID == id);
        }
        public async Task<List<TestAppointment>> GetAllAsync()
        {
            return await _context.TestAppointments.AsNoTracking().ToListAsync();
        }

        public async Task<bool> HasTestAppointmentsAsync(int ldlaID)
        {
            return await _context.TestAppointments.AnyAsync(x => x.LocalDrivingLicenseApplicationID == ldlaID);
        }

        public async Task<bool> IsThereAnActiveScheduledTest(int localDrivingLicenseApplicationID,int testTypeID)
        {
            return await _context.TestAppointments.AnyAsync(t => t.LocalDrivingLicenseApplicationID == localDrivingLicenseApplicationID &&
                t.TestTypeID == testTypeID && !t.IsLocked);
        }

        public async Task<List<TestAppointment>> GetByLDLAAndTestTypeAsync(int localDrivingLicenseApplicationID, int testTypeID)
        {
            return await _context.TestAppointments.Include(x => x.Tests)
                .Where(x => x.LocalDrivingLicenseApplicationID ==  localDrivingLicenseApplicationID && x.TestTypeID == testTypeID)
                .OrderByDescending(x => x.TestAppointmentID).ToListAsync();
        }

        public async Task<bool> LockAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.TestAppointments.FirstOrDefaultAsync(x => x.TestAppointmentID == appointmentId);
            if (appointment == null) return false;
            appointment.IsLocked = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<GetScheduledTestInfoDto?> GetScheduledTestInfoAsync(int testAppointmentID)
        {
            return await _context.TestAppointments.Where(x => x.TestAppointmentID == testAppointmentID)
                .Select(x => new GetScheduledTestInfoDto
                {
                    LocalDrivingLicenseApplicationID = x.LocalDrivingLicenseApplicationID,

                    DrivingClass = x.localDrivingLicenseApplication!.LicenseClass!.ClassName,

                    FullName = x.localDrivingLicenseApplication.Applications!.Person!.FirstName
                    + " " +x.localDrivingLicenseApplication.Applications.Person.SecondName
                    + " " + x.localDrivingLicenseApplication.Applications.Person.LastName,

                    Trial = x.localDrivingLicenseApplication.TestAppointments.Where(a => a.TestTypeID == x.TestTypeID)
                        .SelectMany(a => a.Tests).Count(),

                    AppointmentDate = x.AppointmentDate,

                    PaidFees = x.PaidFees,

                    TestID = x.Tests .Select(t => (int?)t.TestID) .FirstOrDefault()
                }).FirstOrDefaultAsync();
        }
    }
}