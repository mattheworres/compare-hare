#region usings

using System.IO;
using System.Text;
using CompareHare.Api.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace CompareHare.Api.Controllers
{
    [ServiceFilter(typeof(NoCacheFilter))]
    public class SinglePageApplicationController : Controller
    {
        private readonly IHostingEnvironment _env;

        public SinglePageApplicationController(
            IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexHtmlPath = Path.Combine(_env.WebRootPath, "index.html");
            return Content(System.IO.File.ReadAllText(indexHtmlPath), "text/html", Encoding.UTF8);
        }
    }
}
