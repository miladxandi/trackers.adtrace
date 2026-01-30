using Trackers.AdTrace.Models;
using Trackers.AdTrace.Services;

namespace Trackers.AdTrace;

// AdTrace.cs
public static partial class AdTraceSdk
{
#if IOS || ANDROID
    private static IAdTracePlatform? _platform;
    private static AdTraceConfig? _config;
    private static bool _isInitialized;

    /// <summary>
    /// Initialize the SDK - Call this in MauiProgram.cs or App.xaml.cs
    /// </summary>
    public static void Create(AdTraceConfig config)
    {
        _config = config;
        _platform = CreatePlatformInstance();
        _platform.Initialize(config);
        _isInitialized = true;
    }

    /// <summary>
    /// Track an event
    /// </summary>
    public static void TrackEvent(AdTraceEvent adTraceEvent)
    {
        EnsureInitialized();
        _platform!.TrackEvent(adTraceEvent);
    }

    /// <summary>
    /// Enable/Disable tracking
    /// </summary>
    public static void SetEnabled(bool enabled)
    {
        EnsureInitialized();
        _platform!.SetEnabled(enabled);
    }

    /// <summary>
    /// Check if SDK is enabled
    /// </summary>
    public static bool IsEnabled() => _isInitialized && (_platform?.IsEnabled() ?? false);

    /// <summary>
    /// Get AdTrace Device ID (adid)
    /// </summary>
    public static string? GetAdid() => _platform?.GetAdid();

    /// <summary>
    /// Get current Attribution
    /// </summary>
    public static AdTraceAttribution? GetAttribution() => _platform?.GetAttribution();

    /// <summary>
    /// Process Deep Link
    /// </summary>
    public static void AppWillOpenUrl(Uri uri)
    {
        EnsureInitialized();
        _platform!.AppWillOpenUrl(uri);
    }

    /// <summary>
    /// GDPR Forget Me
    /// </summary>
    public static void GdprForgetMe()
    {
        EnsureInitialized();
        _platform!.GdprForgetMe();
    }

    private static void EnsureInitialized()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("AdTrace SDK not initialized. Call AdTraceSdk.Create() first.");
    }

    // Partial method for platform-specific implementation
        private static partial IAdTracePlatform CreatePlatformInstance();
#endif
}