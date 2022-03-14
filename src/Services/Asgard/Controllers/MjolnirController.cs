using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asgard.ActionResults;
using Asgard.Services;

namespace Asgard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MjolnirController : ControllerBase
    {
        private readonly ICurrentHeroService _currentHeroService;
        private readonly IMjolnirWieldingService _mjolnirWieldingService;

        public MjolnirController(ICurrentHeroService currentHeroService,
                                 IMjolnirWieldingService mjolnirWieldingService)
        {
            _currentHeroService = currentHeroService;
            _mjolnirWieldingService = mjolnirWieldingService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var hero = _currentHeroService.Get();

            var isSuccessful = _mjolnirWieldingService.Wield(hero);
            if (isSuccessful)
                return new WorthyResult();

            return new UnworthyResult();
        }
    }
}
