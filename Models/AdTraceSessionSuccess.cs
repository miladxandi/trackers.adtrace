namespace Trackers.AdTrace.Models;

// Models/AdTraceSessionSuccess.cs
public class AdTraceSessionSuccess
{
    public string? Message { get; set; }
    public string? Timestamp { get; set; }
    public string? Adid { get; set; }
    public Dictionary<string, object>? JsonResponse { get; set; }
}