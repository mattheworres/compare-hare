#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.Extensions.Configuration;

#endregion

namespace CompareHare.Api.BackgroundJobs.Configuration
{
    public class BackgroundJobsDashboardConfiguration : IFeatureService
    {
        private readonly IConfiguration _configuration;

        public BackgroundJobsDashboardConfiguration(IConfiguration configuration) {
            _configuration = configuration;
        }

        public bool RequireSsl => bool.Parse(_configuration["background-jobs:dashboard:RequireSsl"]);
        public string Username => _configuration["background-jobs:dashboard:Username"];
        public string Password => _configuration["background-jobs:dashboard:Password"];
        public IEnumerable<string> WhitelistedIpAddresses => _configuration["background-jobs:dashboard:WhitelistedIpAddresses"]?.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()) ?? Enumerable.Empty<string>();
    }
}
