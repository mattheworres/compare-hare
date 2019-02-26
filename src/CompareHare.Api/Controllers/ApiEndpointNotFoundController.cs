#region usings

using Microsoft.AspNetCore.Mvc;

#endregion

namespace CompareHare.Api.Controllers
{
    public class ApiEndpointNotFoundController : Controller
    {
        public IActionResult Index()
        {
            return NotFound();
        }
    }
}
