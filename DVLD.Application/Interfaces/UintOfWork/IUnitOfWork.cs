namespace DVLD.Application.Interfaces.UintOfWork
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> action);

        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}