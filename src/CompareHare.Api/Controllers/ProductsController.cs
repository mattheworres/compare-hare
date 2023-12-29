using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Controllers
{
    public class ProductsController : SpaApiController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("thanks!");
        }
    }

}
