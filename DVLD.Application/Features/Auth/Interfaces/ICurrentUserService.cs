namespace DVLD.Application.Features.Auth.Interfaces
{
    public interface ICurrentUserService
    {
        int AuthUserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}