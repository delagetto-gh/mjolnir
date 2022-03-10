using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Mjolnir.Services;

namespace Mjolnir.Infrastructure
{
    public class CurrentHeroService : ICurrentHeroService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentHeroService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool WieldMjolnir()
        {
            var currUser = _httpContextAccessor?.HttpContext?.User;
            return currUser != null && IsWorthy(currUser);
        }

        private static bool IsWorthy(ClaimsPrincipal currUser) => currUser.HasClaim(AsgardianClaims.Worthiness, "Worthy");
    }
}
