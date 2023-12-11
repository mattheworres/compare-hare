using System.Collections.Generic;
using AngleSharp.Dom;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Services.Interfaces
{
    public interface IParserHelper : IFeatureService
    {
        IEnumerable<string> GetClasses(IElement element);
        bool ElementHasClass(IElement element, string className, bool caseSensitive = true);
        string GetElementClassStartingWith(IElement element, string startingNeedle);
        int ParseFirstIntegerFromString(string text);
        float ParseFirstFloatFromString(string text);
        float ParseCurrencyWithSymbol(string text);
        string RemoveCommasFromString(string text);
    }
}
