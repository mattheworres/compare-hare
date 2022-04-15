using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using AngleSharp.Js;
using CompareHare.Domain.Services.Interfaces;
using Serilog;

namespace CompareHare.Domain.Services
{
    //This is here to help facilitate the mocking of the document contents that AngleSharp
    //provides so as to do "offline" scrapes in unit testing
    public class ParserWrapper : IParserWrapper
    {
        public async Task<IDocument> OpenUrlAsync(string url, IRequester defaultRequester)
        {
            if (defaultRequester == null) {
                var USER_AGENTS = new string[] {
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:98.0) Gecko/20100101 Firefox/98.0",
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.84 Safari/537.36",
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.3 Safari/605.1.15"
                };
                var random = new Random();
                var number = random.Next(3);
                var userAgent = USER_AGENTS[number];
                Log.Logger.Information("Using User Agent of: " + userAgent);
                defaultRequester = new DefaultHttpRequester(userAgent);
                // var handler = new HttpClientHandler{
                //     Proxy = new WebProxy("127.0.0.1", 8888),
                //     UseProxy = true,
                //     UseCookies = false,
                //     AllowAutoRedirect = false
                // };
                // var client = new HttpClient(handler);
                // client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:98.0) Gecko/20100101 Firefox/98.0");
                // var requester = new DefaultHttpRequester("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:98.0) Gecko/20100101 Firefox/98.0");
            }

            var loaderOptions = new LoaderOptions();

            loaderOptions.IsResourceLoadingEnabled = true;

            var angleSharpConfig = Configuration.Default
                // .WithJs()
                .WithCulture("en-US")
                .WithDefaultCookies()
                .WithDefaultLoader(loaderOptions)
                .WithEventLoop()
                // .WithConsoleLogger(ctx => new Logger())
                .With(defaultRequester);
            var angleSharpContext = BrowsingContext.New(angleSharpConfig);
            // angleSharpContext.OpenAsync()
            return await angleSharpContext
                .OpenAsync(url)
                .WaitUntilAvailable()
                ;
        }

        public IDocument OpenUrlSync(string url, IRequester defaultRequester)
        {
            if (defaultRequester == null)
            {
                var USER_AGENTS = new string[] {
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:98.0) Gecko/20100101 Firefox/98.0",
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.84 Safari/537.36",
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.3 Safari/605.1.15"
                };
                var random = new Random();
                var number = random.Next(3);
                var userAgent = USER_AGENTS[number];
                Log.Logger.Information("Using User Agent of: " + userAgent);
                defaultRequester = new DefaultHttpRequester(userAgent);
                // var handler = new HttpClientHandler{
                //     Proxy = new WebProxy("127.0.0.1", 8888),
                //     UseProxy = true,
                //     UseCookies = false,
                //     AllowAutoRedirect = false
                // };
                // var client = new HttpClient(handler);
                // client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:98.0) Gecko/20100101 Firefox/98.0");
                // var requester = new DefaultHttpRequester("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:98.0) Gecko/20100101 Firefox/98.0");
            }

            var loaderOptions = new LoaderOptions();

            loaderOptions.IsResourceLoadingEnabled = true;

            var angleSharpConfig = Configuration.Default
                .WithJs()
                .WithCulture("en-US")
                .WithDefaultCookies()
                .WithDefaultLoader(loaderOptions)
                .WithEventLoop()
                // .WithConsoleLogger(ctx => new Logger())
                .With(defaultRequester);
            var angleSharpContext = BrowsingContext.New(angleSharpConfig);
            // angleSharpContext.OpenAsync()
            return angleSharpContext
                .OpenAsync(url)
                .WaitUntilAvailable()
                .Result;
        }
    }

    public class Logger : IConsoleLogger
    {
        public void Log(object[] values)
        {
            // Serilog.Log.Logger.Information("A#: ", values);
            Console.WriteLine(values);
        }
    }
}
