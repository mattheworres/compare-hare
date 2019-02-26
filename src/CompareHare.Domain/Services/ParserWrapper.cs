using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using CompareHare.Domain.Services.Interfaces;

namespace CompareHare.Domain.Services
{
    //This is here to help facilitate the mocking of the document contents that AngleSharp
    //provides so as to do "offline" scrapes in unit testing
    public class ParserWrapper : IParserWrapper
    {
        public async Task<IDocument> OpenUrlAsync(string url, IRequester defaultRequester)
        {
            if (defaultRequester == null) {
                defaultRequester = new DefaultHttpRequester();
            }

            var angleSharpConfig = Configuration.Default.WithDefaultLoader().With(defaultRequester);
            var angleSharpContext = BrowsingContext.New(angleSharpConfig);
            return await angleSharpContext.OpenAsync(url);
        }
    }
}
