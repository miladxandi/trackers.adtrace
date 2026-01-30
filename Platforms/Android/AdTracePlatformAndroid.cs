using Android.Media;
using Trackers.AdTrace.Models;
using Trackers.AdTrace.Services;

namespace Trackers.AdTrace;

// Platforms/Android/AdTracePlatformAndroid.cs

using Android.Content;
using Android.Provider;

internal class AdTracePlatformAndroid : IAdTracePlatform
{
    private AdTraceConfig? _config;
    private bool _isEnabled = true;
    private string? _adid;
    private AdTraceAttribution? _attribution;
    private readonly HttpClient _httpClient = new();
    
    private const string BaseUrl = "https://app.adtrace.io"; // AdTrace API endpoint

    public void Initialize(AdTraceConfig config)
    {
        _config = config;
        _adid = GetAndroidId();
        
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
        // Handle deep link
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

    private string? GetAndroidId()
    {
        try
        {
            var context = Android.App.Application.Context;
            return Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
        }
        catch
        {
            return Guid.NewGuid().ToString();
        }
    }

    private bool IsFirstLaunch()
    {
        var prefs = Android.App.Application.Context.GetSharedPreferences("adtrace", FileCreationMode.Private);
        var isFirst = !prefs.Contains("installed");
        if (isFirst)
        {
            prefs.Edit()?.PutBoolean("installed", true)?.Apply();
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
                ["os_name"] = "android",
                ["os_version"] = Android.OS.Build.VERSION.Release ?? "",
                ["device_name"] = Android.OS.Build.Model ?? "",
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
                payload["currency"] = adTraceEvent.Currency ?? "IRR";
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

    private async Task ProcessDeepLinkAsync(Uri uri)
    {
        _config?.DeferredDeepLinkReceived?.Invoke(uri);
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
        => new AdTracePlatformAndroid();
}