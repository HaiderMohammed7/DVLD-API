using DVLD.Application.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface IDriverRepository
    {
        Task<Driver?> GetByPersonIdAsync(int personId);
        Task AddAsync(Driver driver);
        Task<List<DriverListDto>> GetAllDrivers();
    }
}