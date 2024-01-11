using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Services.Interfaces
{
    public interface IParserWrapper : IFeatureService
    {
        Task<IDocument> OpenUrlAsync(string url, IRequester defaultHttpRequester, IConfiguration? configuration = null);
        IDocument OpenUrlSync(string url, IRequester defaultHttpRequester, IConfiguration? configuration);
    }
}
