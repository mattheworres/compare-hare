using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace CompareHare.Api.BackgroundJobs.Configuration
{
    public class BackgroundJobsAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly bool _alwaysAllow;

        public BackgroundJobsAuthorizationFilter (bool alwaysAllow) {
            _alwaysAllow = alwaysAllow;
        }

        public bool Authorize([NotNull] DashboardContext context) {
            return _alwaysAllow;
        }
    }
}
