using System;
using System.Security.Claims;

namespace Library.API.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return int.Parse(principal.Identity.Name);
        }
    }
}