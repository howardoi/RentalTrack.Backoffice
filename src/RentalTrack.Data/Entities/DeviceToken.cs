using RentalTrack.Utility.Enums;

namespace RentalTrack.Data.Entities;

public class DeviceToken
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FcmToken { get; set; } = string.Empty;
    public Platform Platform { get; set; }
    public DateTime LastSeenAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
