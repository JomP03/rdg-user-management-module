using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Services.API.Extensions
{
    /// <summary>
    /// Handler for the scope
    /// </summary>
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        /// <summary>
        /// Handle the requirement
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
          AuthorizationHandlerContext context,
          HasScopeRequirement requirement
        )
        {
            // If user does not have the permissions claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Find all permissions
            var permissions = context.User.FindAll(c => c.Type == "permissions" && c.Issuer == requirement.Issuer)
                .Select(c => c.Value);

            // Succeed if the permissions array contains the required permission
            if (permissions.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}