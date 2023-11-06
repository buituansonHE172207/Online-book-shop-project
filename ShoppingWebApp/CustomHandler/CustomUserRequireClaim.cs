using Microsoft.AspNetCore.Authorization;

namespace ShoppingWebApp.CustomHandler
{
    public class CustomUserRequireClaim : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public CustomUserRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }
    }
}

