using DVLD.Application.Interfaces;
using DVLD.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DVLD.API.Extensions
{
    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        private readonly IUserRepository _userRepo;

        public AdminHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            var authUserIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (authUserIdClaim == null)
                return;

            var authUserId = int.Parse(authUserIdClaim.Value);

            var user = await _userRepo.GetByAuthUserIdAsync(authUserId);

            if (user == null)
                return;

            if (user.Role == UserRole.Admin)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}