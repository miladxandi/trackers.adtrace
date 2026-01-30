// Models/AdTraceSessionFailure.cs
namespace Trackers.AdTrace.Models;

public class AdTraceSessionFailure
{
    public string? Message { get; set; }
    public string? Timestamp { get; set; }
    public string? Adid { get; set; }
    public bool WillRetry { get; set; }
    public Dictionary<string, object>? JsonResponse { get; set; }
}