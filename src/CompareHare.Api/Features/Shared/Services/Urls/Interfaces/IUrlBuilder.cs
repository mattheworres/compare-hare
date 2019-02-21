using System.Collections.Generic;

namespace CompareHare.Api.Features.Shared.Urls.Interfaces
{
    public interface IUrlBuilder
    {
        string BuildWebAppUrl(params string[] pathComponents);
        string BuildWebAppUrl(string[] pathComponents, Dictionary<string, string> queryString);
    }
}
