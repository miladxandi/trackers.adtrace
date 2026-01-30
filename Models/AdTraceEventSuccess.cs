// Models/AdTraceEventSuccess.cs
namespace Trackers.AdTrace.Models;

public class AdTraceEventSuccess
{
    public string? EventToken { get; set; }
    public string? Message { get; set; }
    public string? Timestamp { get; set; }
    public string? Adid { get; set; }
    public string? CallbackId { get; set; }
    public Dictionary<string, object>? JsonResponse { get; set; }
}