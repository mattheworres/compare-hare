#region usings

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#endregion

namespace CompareHare.Api.AppStartup
{
    public static class JsonOptionsConfigurator
    {
        public static void Configure(MvcJsonOptions options)
        {
            var serializerSettings = options.SerializerSettings;
            serializerSettings.Converters.Add(new StringEnumConverter());
            serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            serializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
