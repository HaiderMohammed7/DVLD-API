using DVLD.Application.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DVLD.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int AuthUserId => int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)
            ?.Value, out var id) ? id : 0;

        public string Email => User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}