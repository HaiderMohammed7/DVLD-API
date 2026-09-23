using DVLD.Application.DTOs;
using DVLD.Application.Interfaces.Repositories;
using DVLD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Repositories.InternationalLicense
{
    public class InternationalLicenseRepoistory : IInternationalLicenseRepository
    {
        private readonly DVLDDbContext _context;
        public InternationalLicenseRepoistory(DVLDDbContext context)
        {
            _context = context;
        }

        public async Task<GetInternationalLicenseInfoDto?> GetInternationalLicenseInfoAsync(int internationalLicenseId)
        {
            return await _context.InternationalLicenses.Where(x => x.InternationalLicenseID == internationalLicenseId)
                .Select(x => new GetInternationalLicenseInfoDto
                {
                    InternationalLicenseId = x.InternationalLicenseID,
                    ApplicationId = x.ApplicationID,
                    LocalLicenseId = x.IssuedUsingLocalLicenseID,
                    DriverId = x.DriverID,
                    FullName = x.Driver!.Person!.FirstName+ " " + x.Driver.Person.SecondName + " " + x.Driver.Person.LastName,
                    NationalNo = x.Driver.Person.NationalNo,
                    ImagePath = x.Driver.Person.ImagePath,
                    DateOfBirth = x.Driver.Person.DateOfBirth,
                    IssueDate = x.IssueDate,
                    ExpirationDate = x.ExpirationDate,
                    IsActive = x.IsActive,
                    Gendor = x.Driver.Person.Gendor
                }).FirstOrDefaultAsync();
        }

        public async Task<List<GetDriverInternationalLicenseDto>> GetDriverInternationalLicensesAsync(int personId)
        {
            return await _context.InternationalLicenses.Where(il => il.Driver!.PersonID == personId)
                .OrderByDescending(il => il.ExpirationDate)
                .Select(il => new GetDriverInternationalLicenseDto
                {
                    InternationalLicenseId = il.InternationalLicenseID,
                    ApplicationId = il.ApplicationID,
                    LocalLicenseId = il.IssuedUsingLocalLicenseID,
                    IssueDate = il.IssueDate,
                    ExpirationDate = il.ExpirationDate,
                    IsActive = il.IsActive
                }).ToListAsync();
        }
    }
}