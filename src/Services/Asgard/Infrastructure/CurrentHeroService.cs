using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Asgard.Services;
using System.Linq;

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
            var currUser = _httpContextAccessor.HttpContext.User;

            var heroName = GetHeroName(currUser);

            return IsHeroWorthy(heroName);
        }

        private string GetHeroName(ClaimsPrincipal currUser) => currUser.FindFirst(ClaimTypes.Name).Value;

        private bool IsHeroWorthy(string heroName)
        {
            var worthyHeroes = new string[]
            {
              "Thor",
              "Captain America",
              "Black Panther",
              "Loki",
              "Vision",
              "Superman"
            };

            return worthyHeroes.Contains(heroName);
        }
    }
}
