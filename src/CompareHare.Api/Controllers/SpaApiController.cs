#region usings

using CompareHare.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace CompareHare.Api.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(NoCacheFilter))]
    public abstract class SpaApiController : Controller { }
}
