using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mjolnir.Api.ActionResults;

namespace Mjolnir.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MjolnirController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new WorthyResult();
        }
    }
}
