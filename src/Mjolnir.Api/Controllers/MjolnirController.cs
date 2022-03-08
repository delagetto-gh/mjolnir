using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mjolnir.Api.ActionResults;

namespace Mjolnir.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MjolnirController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var worthinessClaim = Infrastructure.AsgardianClaims.Worthiness; 
            if(!User.HasClaim(o => o.Type == worthinessClaim) || User.Claims.First(o => o.Type == worthinessClaim).Value != "Worthy")
                return new UnworthyResult();
            
            return new WorthyResult();
        }
    }
}
