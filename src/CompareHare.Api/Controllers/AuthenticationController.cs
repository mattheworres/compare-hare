using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationControllerController : ControllerBase
    {
        private readonly ILogger<Controller> _logger;
        public AuthenticationControllerController(ILogger<Controller> logger)
        {
            _logger = logger;
        }

        //todo: sign-in, log-out
    }

}