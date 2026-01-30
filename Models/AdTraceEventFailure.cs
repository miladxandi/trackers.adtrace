// Models/AdTraceEventFailure.cs
namespace Trackers.AdTrace.Models;

public class AdTraceEventFailure
{
    public string? EventToken { get; set; }
    public string? Message { get; set; }
    public string? Timestamp { get; set; }
    public string? Adid { get; set; }
    public string? CallbackId { get; set; }
    public bool WillRetry { get; set; }
    public Dictionary<string, object>? JsonResponse { get; set; }
}