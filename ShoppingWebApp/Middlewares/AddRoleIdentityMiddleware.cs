using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using System.Security.Claims;

namespace ShoppingWebApp.Middlewares
{
    public class AddRoleIdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public AddRoleIdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                var claims = httpContext.User.Claims;
                var role = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                if (role == null)
                {
                    IUserRepository _userRepository = new UserRepository();
                    var email = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
                    User user = _userRepository.GetUser(email);

                    if (user != null)
                    {
                        var newClaims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Role, user.RoleNavigation.RoleName)
                        };
                        var claimIdentity = new ClaimsIdentity(newClaims);
                        httpContext.User.AddIdentity(claimIdentity);
                    }
                }
            }
            await _next(httpContext);
        }
    }
}
