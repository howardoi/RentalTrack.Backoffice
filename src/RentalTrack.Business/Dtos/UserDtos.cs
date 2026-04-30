namespace RentalTrack.Business.Dtos;

public class UserListItemDto
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Provider { get; set; }
    public int PropertyCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class UserSearchResultDto
{
    public IEnumerable<UserListItemDto> Items { get; set; } = Array.Empty<UserListItemDto>();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class UserDetailDto
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public string? Provider { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public List<UserPropertyDto> Properties { get; set; } = new();
}

public class UserPropertyDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal MonthlyRentalAmount { get; set; }
    public string? TenantName { get; set; }
}
