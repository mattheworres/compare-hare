using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using AngleSharp.Js;
using CompareHare.Domain.Services.Interfaces;

namespace CompareHare.Domain.Services
{
    //This is here to help facilitate the mocking of the document contents that AngleSharp
    //provides so as to do "offline" scrapes in unit testing
    public class ParserWrapper : IParserWrapper
    {
        private readonly IRequesterHelper _requesterHelper;

        public ParserWrapper(IRequesterHelper requesterHelper)
        {
            _requesterHelper = requesterHelper;
        }

        public async Task<IDocument> OpenUrlAsync(string url, IRequester defaultRequester, IConfiguration? configuration = null)
        {
            if (defaultRequester == null)
            {
                defaultRequester = _requesterHelper.GetDefaultRequester();
            }

            var loaderOptions = _requesterHelper.GetDefaultLoaderOptions();

            loaderOptions.IsResourceLoadingEnabled = true;

            if (configuration == null)
            {
                configuration = Configuration.Default
                .WithJs()
                .WithCulture("en-US")
                .WithDefaultCookies()
                .WithEventLoop()
                .With(defaultRequester)
                .WithDefaultLoader(loaderOptions);
                ;
            }

            var angleSharpContext = BrowsingContext.New(configuration);

            return await angleSharpContext
                .OpenAsync(url)
                .WaitUntilAvailable()
                ;
        }

        public IDocument OpenUrlSync(string url, IRequester defaultRequester, IConfiguration? configuration = null)
        {
            if (defaultRequester == null)
            {
                defaultRequester = _requesterHelper.GetDefaultRequester();
            }

            var loaderOptions = _requesterHelper.GetDefaultLoaderOptions();

            if (configuration == null)
            {
                configuration = Configuration.Default
                .WithJs()
                .WithCulture("en-US")
                .WithDefaultCookies()
                .WithEventLoop()
                .With(defaultRequester)
                .WithDefaultLoader(loaderOptions);
                ;
            }

            var angleSharpContext = BrowsingContext.New(configuration);

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
