using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Asgard.Services;
using Asgard.Models;

namespace Asgard.Infrastructure
{
    public class CurrentHeroService : ICurrentHeroService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentHeroService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Hero Get()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user == null)
                throw new Exception("Unable to obtain user from current http request context.");

            var heroName = GetHeroName(user);
            return new Hero(heroName);
        }

        private static string GetHeroName(ClaimsPrincipal currUser) => currUser.FindFirst(ClaimTypes.Name).Value;
    }
}
