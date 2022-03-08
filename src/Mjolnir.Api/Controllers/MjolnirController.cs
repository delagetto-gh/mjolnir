using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mjolnir.Api.ActionResults;
using Mjolnir.Api.Services;

namespace Mjolnir.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MjolnirController : ControllerBase
    {
        private readonly ICurrentHeroService _heroService;

        public MjolnirController(ICurrentHeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var isSuccess = _heroService.WieldMjolnir();
            if (isSuccess)
                return new WorthyResult();

            return new UnworthyResult();
        }
    }
}
