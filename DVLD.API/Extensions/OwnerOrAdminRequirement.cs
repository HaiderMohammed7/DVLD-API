using Microsoft.AspNetCore.Authorization;

namespace DVLD.API.Extensions
{
    public class OwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}