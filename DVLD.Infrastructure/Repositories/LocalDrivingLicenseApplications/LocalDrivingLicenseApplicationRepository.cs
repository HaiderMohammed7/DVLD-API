using DVLD.Application.Features.Tests.DTOs;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.LocalDrivingLicenseApplications
{
    public class LocalDrivingLicenseApplicationRepository : ILocalDrivingLicenseApplicationRepository
    {
        private readonly DVLDDbContext _context;

        public LocalDrivingLicenseApplicationRepository(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LocalDrivingLicenseApplication ldla)
        {
            await _context.LocalDrivingLicenseApplications.AddAsync(ldla);
        }
        public async Task DeleteAsync(LocalDrivingLicenseApplication ldla)
        {
            _context.LocalDrivingLicenseApplications.Remove(ldla);
        }

        public async Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId)
        {
            return await _context.LocalDrivingLicenseApplications.AnyAsync(x =>
                    x.LicenseClassID == licenseClassId &&
                    x.Applications.ApplicantPersonID == personId &&
                    x.Applications.ApplicationStatus != ApplicationStatus.Cancelled);
        }
        public async Task<bool> HasActiveApplicationAsync(int personId, int licenseClassId,int excludeApplicationId)
        {
            return await _context.LocalDrivingLicenseApplications.AnyAsync(x =>
                x.LocalDrivingLicenseApplicationID != excludeApplicationId &&
                x.LicenseClassID == licenseClassId &&
                x.Applications.ApplicantPersonID == personId &&
                x.Applications.ApplicationStatus != ApplicationStatus.Cancelled);
        }

        public async Task<LocalDrivingLicenseApplication?> GetByIdAsync(int id)
        {
            return await _context.LocalDrivingLicenseApplications.Include(x => x.Applications)
                    .ThenInclude(x => x.Licenses).Include(x => x.LicenseClass).Include(x => x.TestAppointments)
                    .ThenInclude(x => x.Tests).FirstOrDefaultAsync(x => x.LocalDrivingLicenseApplicationID == id);
        }
        public async Task<List<LocalDrivingLicenseApplication>> GetAllAsync()
        {
            return await _context.LocalDrivingLicenseApplications.Include(x => x.Applications)
                    .ThenInclude(x => x.Person).Include(x => x.LicenseClass).Include(x => x.TestAppointments)
                    .ThenInclude(x => x.Tests).ToListAsync();
        }

        public async Task<bool> DoesPassTestType(int localDrivingLicenseApplicationID,int testTypeID)
        {
            var appointment = await _context.TestAppointments.Where(ta => ta.LocalDrivingLicenseApplicationID == localDrivingLicenseApplicationID && ta.TestTypeID == testTypeID)
                .OrderByDescending(ta => ta.TestAppointmentID).Select(ta => new
                {
                    HasTest = ta.Tests.Any(),
                    TestResult = ta.Tests.OrderByDescending(t => t.TestID)
                    .Select(t => t.TestResult).FirstOrDefault()
                }).FirstOrDefaultAsync();

            return appointment?.HasTest == true && appointment.TestResult;
        }

        public async Task<GetScheduleTestInfoDto?> GetScheduleTestInfoAsync(int localDrivingLicenseApplicationID, int testTypeID)
        {
            return await _context.LocalDrivingLicenseApplications.Where(x => x.LocalDrivingLicenseApplicationID == localDrivingLicenseApplicationID)
                .Select(x => new GetScheduleTestInfoDto
                {
                    LocalDrivingLicenseApplicationID = x.LocalDrivingLicenseApplicationID,

                    DrivingClass = x.LicenseClass!.ClassName,

                    FullName = x.Applications!.Person!.FirstName + " " +
                    x.Applications.Person.SecondName + " " + x.Applications.Person.LastName,

                    Trial = x.TestAppointments.Where(a => a.TestTypeID == testTypeID).SelectMany(a => a.Tests).Count(),

                    TestFees = _context.TestTypes.Where(t => t.TestTypeID == testTypeID).Select(t => t.TestTypeFees).FirstOrDefault(),

                    RetakeApplicationFees = 0,
                    TotalFees = 0,
                    RetakeTestApplicationID = null
                }).FirstOrDefaultAsync();
        }
    }
}