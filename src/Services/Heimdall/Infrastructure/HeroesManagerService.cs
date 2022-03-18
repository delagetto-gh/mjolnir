using System;
using System.Linq;
using System.Threading.Tasks;
using Heimdall.Exceptions;
using Heimdall.Services;
using Microsoft.AspNetCore.Identity;

namespace Heimdall.Infrastructure
{
    public class HeroesManagerService : IHeroRegistrationService,
                                        IAsgardPassIssuerService
    {
        private readonly UserManager<IdentityUser> _usersManager;

        public HeroesManagerService(UserManager<IdentityUser> usersManager)
        {
            _usersManager = usersManager;
        }

        public async Task RegisterHeroAync(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            var identity = new IdentityUser
            {
                UserName = name
            };

            var existingHero = await _usersManager.FindByNameAsync(name);
            if (existingHero != null)
                throw new HeroNameTakenException(name);

            await _usersManager.CreateAsync(identity, password);
        }
    }
}