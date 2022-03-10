using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mjolnir.ActionResults;
using Mjolnir.Services;

namespace Mjolnir.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MjolnirController : ControllerBase
    {
        private readonly ICurrentHeroService _currentHeroService;

        public MjolnirController(ICurrentHeroService heroService)
        {
            _currentHeroService = heroService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var isSuccess = _currentHeroService.WieldMjolnir();
            if (isSuccess)
                return new WorthyResult();

            return new UnworthyResult();
        }
    }
}
