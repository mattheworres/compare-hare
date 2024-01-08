using System.Net;
using AngleSharp.Io;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Services.Interfaces
{
    public interface IRequesterHelper : IFeatureService
    {
         string[] GetUserAgentStrings();
         string GetRandomUserAgentString();
         string GetCookieString(Dictionary<string, string> cookies);
         WebHeaderCollection GetHeaders(string userAgent, string cookieString);
         LoaderOptions GetDefaultLoaderOptions();
         IRequester GetDefaultRequester();
        IRequester GetRequester(WebHeaderCollection headers);
    }
}
