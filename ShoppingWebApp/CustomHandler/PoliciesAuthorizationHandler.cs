using Microsoft.AspNetCore.Authorization;

namespace ShoppingWebApp.CustomHandler
{
    public class PoliciesAuthorizationHandler : AuthorizationHandler<CustomUserRequireClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomUserRequireClaim requirement)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var claims = context.User.Claims;
                var hasClaim = claims.Any(x => x.Type == requirement.ClaimType);
                if (!hasClaim)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
                var role = claims.FirstOrDefault(c => c.Type.Equals(requirement.ClaimType));
                if (role != null && role.Value.Equals("Admin"))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
