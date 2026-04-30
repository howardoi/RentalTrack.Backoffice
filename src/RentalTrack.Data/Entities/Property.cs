namespace RentalTrack.Data.Entities;

public class Property
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal MonthlyRentalAmount { get; set; }
    public string? TenantName { get; set; }
    public DateTime? TenancyStartDate { get; set; }
    public DateTime? CukaiTaksiranDueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
