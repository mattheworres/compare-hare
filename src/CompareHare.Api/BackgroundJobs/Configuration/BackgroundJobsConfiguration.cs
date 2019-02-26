#region usings

using CompareHare.Domain.Features.Interfaces;
using Microsoft.Extensions.Configuration;
using NodaTime;

#endregion

namespace CompareHare.Api.BackgroundJobs.Configuration
{
    public class BackgroundJobsConfiguration : IFeatureService
    {
        private readonly IDateTimeZoneProvider _dateTimeZoneProvider;
        private readonly IConfiguration _configuration;

        public BackgroundJobsConfiguration(
            IDateTimeZoneProvider dateTimeZoneProvider,
            IConfiguration configuration)
        {
            _dateTimeZoneProvider = dateTimeZoneProvider;
            _configuration = configuration;
        }

        public DateTimeZone TimeZone => _dateTimeZoneProvider.GetZoneOrNull(_configuration["background-jobs:TimeZone"]);
    }
}
