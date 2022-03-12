using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public MjolnirController(ICurrentHeroService heroService)
        {
            _currentHeroService = heroService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var isSuccess = _currentHeroService.WieldAsgard();
            if (isSuccess)
                return new WorthyResult();

            return new UnworthyResult();
        }
    }
}
