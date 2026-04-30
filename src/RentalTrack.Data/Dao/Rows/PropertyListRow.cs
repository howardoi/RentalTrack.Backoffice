namespace RentalTrack.Data.Dao;

public class PropertyListRow
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal MonthlyRentalAmount { get; set; }
    public string? TenantName { get; set; }
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerDisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
