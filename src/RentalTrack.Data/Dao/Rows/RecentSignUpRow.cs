namespace RentalTrack.Data.Dao;

public class RecentSignUpRow
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Provider { get; set; }
}
