
using Microsoft.AspNetCore.Authorization;

namespace PolicyProvider
{
    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
