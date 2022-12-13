using System.Security.Claims;

namespace ContactBook.WebAPI.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Email(this ClaimsPrincipal user)
            => user.Identity.Name;
    }
}
