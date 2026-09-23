using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DVLD.Application.Features.Auth.Interfaces;

namespace DVLD.Application.Features.Auth.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int AuthUserId
        {
            get
            {
                if (!IsAuthenticated)
                    return 0;

                var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return int.TryParse(userIdClaim, out var id) ? id : 0;
            }
        }

        public string? Email =>
            IsAuthenticated
                ? User?.FindFirst(ClaimTypes.Email)?.Value
                : null;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;
    }
}