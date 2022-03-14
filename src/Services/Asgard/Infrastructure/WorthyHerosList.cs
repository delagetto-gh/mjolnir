using System.Linq;
using Asgard.Services;

namespace Asgard.Infrastructure
{
    public class WorthyHerosList : IWorthyHerosList
    {
        public bool Contains(string heroName)
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