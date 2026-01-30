using Microsoft.Extensions.Logging;

namespace Trackers.AdTrace.Models;

// Models/AdTraceConfig.cs
public class AdTraceConfig
{
    public string AppToken { get; set; }
    public AdTraceEnvironment Environment { get; set; }
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public bool SendInBackground { get; set; } = false;
    public TimeSpan? DelayStart { get; set; }
    
    // Callbacks
    public Action<AdTraceAttribution>? AttributionChanged { get; set; }
    public Action<AdTraceSessionSuccess>? SessionTrackingSucceeded { get; set; }
    public Action<AdTraceSessionFailure>? SessionTrackingFailed { get; set; }
    public Action<AdTraceEventSuccess>? EventTrackingSucceeded { get; set; }
    public Action<AdTraceEventFailure>? EventTrackingFailed { get; set; }
    
    // Deep Link
    public Action<Uri>? DeferredDeepLinkReceived { get; set; }

    public AdTraceConfig(string appToken, AdTraceEnvironment environment)
    {
        AppToken = appToken;
        Environment = environment;
    }
}

public enum AdTraceEnvironment
{
    Sandbox,
    Production
}