using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Controllers
{
    [Authorize]
    [EnableCors("myCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class SpaApiController : ControllerBase {}

}
