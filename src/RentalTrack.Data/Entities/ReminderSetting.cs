using RentalTrack.Utility.Enums;

namespace RentalTrack.Data.Entities;

public class ReminderSetting
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public ReminderType ReminderType { get; set; }
    public bool IsEnabled { get; set; }
    public short OffsetDays { get; set; }
    public bool ChannelPush { get; set; }
    public bool ChannelEmail { get; set; }
    public DateTime UpdatedAt { get; set; }
}
