using Trackers.AdTrace.Models;
using Trackers.AdTrace.Services;
using Foundation;
using UIKit;

namespace Trackers.AdTrace;

// Platforms/iOS/AdTracePlatformiOS.cs
internal class AdTracePlatformiOS : IAdTracePlatform
{
    private AdTraceConfig? _config;
    private bool _isEnabled = true;
    private string? _adid;
    private AdTraceAttribution? _attribution;
    private readonly HttpClient _httpClient = new();
    
    private const string BaseUrl = "https://app.adtrace.io";

    public void Initialize(AdTraceConfig config)
    {
        _config = config;
        _adid = GetDeviceId();
        
        // Track install/session
        _ = TrackSessionAsync(isInstall: IsFirstLaunch());
    }

    public void TrackEvent(AdTraceEvent adTraceEvent)
    {
        if (!_isEnabled) return;
        _ = TrackEventAsync(adTraceEvent);
    }

    public void SetEnabled(bool enabled) => _isEnabled = enabled;

    public bool IsEnabled() => _isEnabled;

    public string? GetAdid() => _adid;

    public AdTraceAttribution? GetAttribution() => _attribution;

    public void AppWillOpenUrl(Uri uri)
    {
        _ = ProcessDeepLinkAsync(uri);
    }

    public void GdprForgetMe()
    {
        _ = SendGdprForgetMeAsync();
    }

    public void OnResume()
    {
        if (!_isEnabled) return;
        _ = TrackSessionAsync(isInstall: false);
    }

    public void OnPause()
    {
        // Optional: track session end
    }

    #region Private Methods

    private string GetDeviceId()
    {
        try
        {
            // Use IdentifierForVendor as device identifier
            return UIDevice.CurrentDevice.IdentifierForVendor?.AsString() 
                   ?? Guid.NewGuid().ToString();
        }
        catch
        {
            return Guid.NewGuid().ToString();
        }
    }

    private bool IsFirstLaunch()
    {
        var key = "adtrace_installed";
        var isFirst = !NSUserDefaults.StandardUserDefaults.BoolForKey(key);
        if (isFirst)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(true, key);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }
        return isFirst;
    }

    private async Task TrackSessionAsync(bool isInstall)
    {
        try
        {
            var endpoint = isInstall ? "/sdk/install" : "/sdk/session";
            var payload = new Dictionary<string, string>
            {
                ["app_token"] = _config!.AppToken,
                ["device_id"] = _adid ?? "",
                ["os_name"] = "ios",
                ["os_version"] = UIDevice.CurrentDevice.SystemVersion,
                ["device_name"] = UIDevice.CurrentDevice.Model,
                ["environment"] = _config.Environment.ToString().ToLower()
            };

            var response = await _httpClient.PostAsync(
                $"{BaseUrl}{endpoint}",
                new FormUrlEncodedContent(payload));

            if (response.IsSuccessStatusCode)
            {
                _config.SessionTrackingSucceeded?.Invoke(new AdTraceSessionSuccess
                {
                    Adid = _adid,
                    Message = "Session tracked successfully",
                    Timestamp = DateTime.UtcNow.ToString("O")
                });
            }
        }
        catch (Exception ex)
        {
            _config?.SessionTrackingFailed?.Invoke(new AdTraceSessionFailure
            {
                Message = ex.Message,
                WillRetry = true
            });
        }
    }

    private async Task TrackEventAsync(AdTraceEvent adTraceEvent)
    {
        try
        {
            var payload = new Dictionary<string, string>
            {
                ["app_token"] = _config!.AppToken,
                ["event_token"] = adTraceEvent.EventToken,
                ["device_id"] = _adid ?? ""
            };

            if (adTraceEvent.Revenue.HasValue)
            {
                payload["revenue"] = adTraceEvent.Revenue.Value.ToString();
                payload["currency"] = adTraceEvent.Currency ?? "USD";
            }

            var response = await _httpClient.PostAsync(
                $"{BaseUrl}/sdk/event",
                new FormUrlEncodedContent(payload));

            if (response.IsSuccessStatusCode)
            {
                _config.EventTrackingSucceeded?.Invoke(new AdTraceEventSuccess
                {
                    EventToken = adTraceEvent.EventToken,
                    Adid = _adid,
                    Timestamp = DateTime.UtcNow.ToString("O")
                });
            }
        }
        catch (Exception ex)
        {
            _config?.EventTrackingFailed?.Invoke(new AdTraceEventFailure
            {
                EventToken = adTraceEvent.EventToken,
                Message = ex.Message,
                WillRetry = true
            });
        }
    }

    private Task ProcessDeepLinkAsync(Uri uri)
    {
        _config?.DeferredDeepLinkReceived?.Invoke(uri);
        return Task.CompletedTask;
    }

    private async Task SendGdprForgetMeAsync()
    {
        var payload = new Dictionary<string, string>
        {
            ["app_token"] = _config!.AppToken,
            ["device_id"] = _adid ?? ""
        };

        await _httpClient.PostAsync($"{BaseUrl}/gdpr/forget", new FormUrlEncodedContent(payload));
        _isEnabled = false;
    }

    #endregion
}

public static partial class AdTraceSdk
{
    private static partial IAdTracePlatform CreatePlatformInstance()
        => new AdTracePlatformiOS();
}
