using Trackers.AdTrace.Models;

namespace Trackers.AdTrace.Services;

// Services/IAdTracePlatform.cs
internal interface IAdTracePlatform
{
    void Initialize(AdTraceConfig config);
    void TrackEvent(AdTraceEvent adTraceEvent);
    void SetEnabled(bool enabled);
    bool IsEnabled();
    string? GetAdid();
    AdTraceAttribution? GetAttribution();
    void AppWillOpenUrl(Uri uri);
    void GdprForgetMe();
    void OnResume();
    void OnPause();
}