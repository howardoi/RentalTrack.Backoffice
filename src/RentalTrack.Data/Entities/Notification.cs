using RentalTrack.Utility.Enums;

namespace RentalTrack.Data.Entities;

public class Notification
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long? PropertyId { get; set; }
    public ReminderType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
