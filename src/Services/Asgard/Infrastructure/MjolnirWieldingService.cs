using Asgard.Models;
using Asgard.Services;

namespace Asgard.Infrastructure
{
    public class MjolnirWieldingService : IMjolnirWieldingService
    {
        private readonly IWorthyHerosList _worthyHeroesList;

        public MjolnirWieldingService(IWorthyHerosList worthyHeroesList)
        {
            _worthyHeroesList = worthyHeroesList;
        }

        public bool Wield(Hero hero) => IsHeroWorthy(hero.Name);
        
        private bool IsHeroWorthy(string name) => _worthyHeroesList.Contains(name);
    }
}