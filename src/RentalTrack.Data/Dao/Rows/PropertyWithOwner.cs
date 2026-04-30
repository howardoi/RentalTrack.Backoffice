namespace RentalTrack.Data.Dao;

public class PropertyWithOwner
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal MonthlyRentalAmount { get; set; }
    public string? TenantName { get; set; }
    public DateTime? TenancyStartDate { get; set; }
    public DateTime? CukaiTaksiranDueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerDisplayName { get; set; } = string.Empty;
}
