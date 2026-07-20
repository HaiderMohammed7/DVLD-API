namespace DVLD.Domain.Contracts
{
    public interface IOwnable<TKey>
    {
        TKey OwnerId { get; }
    }
}