#region usings

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace CompareHare.Api.Filters
{
    public class NoCacheFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Response.Headers.ContainsKey("Cache-control"))
            {
                context.HttpContext.Response.Headers.Add("Cache-Control", "no-store, must-revalidate, no-cache, max-age=0, private");
            }

            await next();
        }
    }
}
