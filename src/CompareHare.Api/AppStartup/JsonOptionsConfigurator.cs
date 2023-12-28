using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CompareHare.Api.AppStartup;

public static class JsonOptionsConfigurator
{
    public static void Configure(MvcNewtonsoftJsonOptions options)
    {
        var serializerSettings = options.SerializerSettings;
        serializerSettings.Converters.Add(new StringEnumConverter());
        serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        serializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
        serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }
}