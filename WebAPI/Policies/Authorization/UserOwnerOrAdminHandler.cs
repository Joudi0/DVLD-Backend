using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Authorization
{
    public class UserOwnerOrAdminHandler : AuthorizationHandler<UserOwnerOrAdminRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            UserOwnerOrAdminRequirement requirement, 
            int resourceUserId)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            string currentUserIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(currentUserIdClaim, out int authenticatedUserId) &&
                authenticatedUserId == resourceUserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}