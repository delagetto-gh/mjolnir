using System.Threading.Tasks;
using Heimdall.Exceptions;
using Heimdall.Requests;
using Heimdall.Services;
using Microsoft.AspNetCore.Mvc;

namespace Heimdall.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroRegistrationService _heroRegistrationService;
        private readonly IAsgardPassIssuerService _asgardPassIssuerService;

        public HeroesController(IHeroRegistrationService heroRegistrationService,
                                IAsgardPassIssuerService asgardPassIssuerService)
        {
            _heroRegistrationService = heroRegistrationService;
            _asgardPassIssuerService = asgardPassIssuerService;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterHero(RegisterHeroRequest request)
        {
            try
            {
                await _heroRegistrationService.RegisterHeroAync(request.HeroName, request.Password);
                return Created(uri: string.Empty, value: null);
            }
            catch (HeroNameTakenException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}