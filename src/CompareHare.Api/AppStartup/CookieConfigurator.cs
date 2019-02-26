using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;

namespace CompareHare.Api.AppStartup
{
    public static class CookieConfigurator
    {
        public static void Configure(CookieAuthenticationOptions options, IHostingEnvironment hostingEnvironment)
        {
            options.Cookie.HttpOnly = hostingEnvironment.IsProduction();
        }
    }
}
