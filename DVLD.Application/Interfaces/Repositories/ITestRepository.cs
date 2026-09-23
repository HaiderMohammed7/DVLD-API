using DVLD.Domain.Entities;

namespace DVLD.Application.Interfaces.Repositories
{
    public interface ITestRepository
    {
        Task AddAsync(Test test);

        Task<Test?> GetByAppointmentIdAsync(int appointmentId);
        Task<Test?> GetByIdAsync(int testId);
    }
}