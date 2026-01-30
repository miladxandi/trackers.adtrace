namespace Trackers.AdTrace.Models;

// Models/AdTraceEvent.cs
public class AdTraceEvent
{
    public string EventToken { get; }
    public decimal? Revenue { get; private set; }
    public string? Currency { get; private set; }
    public Dictionary<string, string> CallbackParameters { get; } = new();
    public Dictionary<string, string> PartnerParameters { get; } = new();
    public string? TransactionId { get; private set; }

    public AdTraceEvent(string eventToken)
    {
        EventToken = eventToken;
    }

    public void SetRevenue(decimal amount, string currency)
    {
        Revenue = amount;
        Currency = currency;
    }

    public void AddCallbackParameter(string key, string value) 
        => CallbackParameters[key] = value;

    public void AddPartnerParameter(string key, string value) 
        => PartnerParameters[key] = value;

    public void SetTransactionId(string transactionId) 
        => TransactionId = transactionId;
}