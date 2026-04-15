namespace DVLD.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int AuthUserId { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
    }
}