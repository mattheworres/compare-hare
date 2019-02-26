using System.Collections.Generic;
using System.Linq;
using System.Net;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CompareHare.Api.Features.Shared.Urls
{
    public class UrlBuilder : Interfaces.IUrlBuilder, IFeatureService
    {
        private readonly IConfiguration _configuration;

        public UrlBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BuildWebAppUrl(string[] pathComponents)
        {
            return BuildWebAppUrl(pathComponents, null);
        }

        public string BuildWebAppUrl(string[] pathComponents, Dictionary<string, string> queryString)
        {
            var partialUrl = string.Join("/", pathComponents.Select(WebUtility.UrlEncode));
            var queryUrl = queryString == null ? "" : string.Join(";", queryString.Select(q => $"{q.Key}={WebUtility.UrlEncode(q.Value)}"));

            if (!partialUrl.EndsWith('/'))
            {
                partialUrl = "/" + partialUrl;
            }
            if (!string.IsNullOrWhiteSpace(queryUrl))
            {
                partialUrl += $"?{queryUrl}";
            }

            return $"{_configuration["webAppUrl"]}{partialUrl}";
        }
    }
}
