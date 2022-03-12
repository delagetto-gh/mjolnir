using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Asgard.Services;

namespace Asgard.Infrastructure
{
    public class CurrentHeroService : ICurrentHeroService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentHeroService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool WieldAsgard()
        {
            var currUser = _httpContextAccessor?.HttpContext?.User;
            return currUser != null && IsWorthy(currUser);
        }

        private static bool IsWorthy(ClaimsPrincipal currUser) => currUser.HasClaim(AsgardianClaims.Worthiness, "Worthy");
    }
}
