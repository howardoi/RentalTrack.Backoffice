namespace RentalTrack.Backoffice.Services;

public interface IEventLogService
{
    void Append(string level, string type, string message, string? ip = null);
    IReadOnlyList<LogEntry> Query(string? query, string? level, string? type, DateTime? since, int limit);
}

public class LogEntry
{
    public DateTime Time { get; set; }
    public string Level { get; set; } = "info";
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Ip { get; set; }
}
