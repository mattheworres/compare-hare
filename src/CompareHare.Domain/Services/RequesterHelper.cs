using System.Net;
using AngleSharp.Io;
using CompareHare.Domain.Services.Interfaces;

namespace CompareHare.Domain.Services
{
    public class RequesterHelper : IRequesterHelper
    {
        public string GetCookieString(Dictionary<string, string> cookies)
        {
            return cookies != null
                ? string.Join(";", cookies.Select(c => $"{c.Key}={c.Value}"))
                : "region=east";
        }

        public WebHeaderCollection GetHeaders(string userAgent, string cookieString)
        {
            var headers = new WebHeaderCollection
            {
                { "User-Agent", userAgent },
                { "Cookie", cookieString }
            };

            return headers;
        }

        public string[] GetUserAgentStrings()
        {
            return new string[] {
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:98.0) Gecko/20100101 Firefox/98.0",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.84 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.3 Safari/605.1.15"
            };
        }

        public string GetRandomUserAgentString()
        {
            var uaStrings = GetUserAgentStrings();
            var random = new Random();
            var number = random.Next(uaStrings.Count());

            return uaStrings[number];
        }

        public LoaderOptions GetDefaultLoaderOptions()
        {
            var options = new LoaderOptions();
            options.IsResourceLoadingEnabled = true;

            return options;
        }

        public IRequester GetDefaultRequester()
        {
            var userAgent = GetRandomUserAgentString();
            var cookieString = GetCookieString(null);
            var headers = GetHeaders(userAgent, cookieString);
            return new DefaultHttpRequester(userAgent, (_) => {
                _.AllowReadStreamBuffering = true;
                _.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                _.Headers = headers;
                _.CookieContainer = new CookieContainer(50);
            });
        }

        public IRequester GetRequester(WebHeaderCollection headers) {
            var userAgent = GetRandomUserAgentString();
            return new DefaultHttpRequester(userAgent, (_) => {
                _.AllowReadStreamBuffering = true;
                _.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                _.Headers = headers;
                _.CookieContainer = new CookieContainer(50);
            });
        }
    }
}
