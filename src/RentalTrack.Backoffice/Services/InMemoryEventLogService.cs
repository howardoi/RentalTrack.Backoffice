using System.Collections.Concurrent;

namespace RentalTrack.Backoffice.Services;

public class InMemoryEventLogService : IEventLogService
{
    private readonly ConcurrentQueue<LogEntry> _entries = new();
    private const int MaxEntries = 5000;

    public void Append(string level, string type, string message, string? ip = null)
    {
        _entries.Enqueue(new LogEntry
        {
            Time = DateTime.UtcNow,
            Level = level,
            Type = type,
            Message = message,
            Ip = ip,
        });
        while (_entries.Count > MaxEntries && _entries.TryDequeue(out _)) { }
    }

    public IReadOnlyList<LogEntry> Query(string? query, string? level, string? type, DateTime? since, int limit)
    {
        IEnumerable<LogEntry> q = _entries.ToArray();
        if (!string.IsNullOrWhiteSpace(level)) q = q.Where(e => string.Equals(e.Level, level, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(type))  q = q.Where(e => string.Equals(e.Type, type, StringComparison.OrdinalIgnoreCase));
        if (since.HasValue)                     q = q.Where(e => e.Time > since.Value);
        if (!string.IsNullOrWhiteSpace(query)) q = q.Where(e => e.Message.Contains(query, StringComparison.OrdinalIgnoreCase));
        return q.OrderByDescending(e => e.Time).Take(limit).ToList();
    }
}
