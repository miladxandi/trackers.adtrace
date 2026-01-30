namespace Trackers.AdTrace.Models;

// Models/AdTraceAttribution.cs
public class AdTraceAttribution
{
    public string? TrackerToken { get; set; }
    public string? TrackerName { get; set; }
    public string? Network { get; set; }
    public string? Campaign { get; set; }
    public string? Adgroup { get; set; }
    public string? Creative { get; set; }
    public string? ClickLabel { get; set; }
    public string? Adid { get; set; }
}