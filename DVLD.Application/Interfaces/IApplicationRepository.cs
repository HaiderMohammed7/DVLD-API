using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces
{
    public interface IApplicationRepository
    {
        Task AddAsync(Applications application);
    }
}