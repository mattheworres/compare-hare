using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Controllers
{
    [Authorize(AuthenticationSchemes = SharedAuthConstants.IdentityApplicationScheme)]
    [EnableCors("myCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class SpaApiController : ControllerBase {}

    public class SharedAuthConstants
    {
        public const string IdentityApplicationScheme = "Identity.Application";
    }
}
