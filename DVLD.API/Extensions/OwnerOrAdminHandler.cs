using DVLD.Application.Interfaces;
using DVLD.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DVLD.API.Extensions
{
    public class OwnerOrAdminHandler : AuthorizationHandler<OwnerOrAdminRequirement, int>
    {
        private readonly IUserRepository _userRepo;

        public OwnerOrAdminHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,OwnerOrAdminRequirement requirement, int personId)
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

            if (user.PersonID == personId)
            {
                context.Succeed(requirement);
            }
        }
    }
}