namespace RentalTrack.Business.Dtos;

public class PropertyListItemDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal MonthlyRentalAmount { get; set; }
    public string? TenantName { get; set; }
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PropertySearchResultDto
{
    public IEnumerable<PropertyListItemDto> Items { get; set; } = Array.Empty<PropertyListItemDto>();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class PropertyDetailDto
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
    public string OwnerName { get; set; } = string.Empty;
}
