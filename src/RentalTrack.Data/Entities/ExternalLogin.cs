using RentalTrack.Utility.Enums;

namespace RentalTrack.Data.Entities;

public class ExternalLogin
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public Provider Provider { get; set; }
    public string ProviderSubject { get; set; } = string.Empty;
    public DateTime LinkedAt { get; set; }
}
