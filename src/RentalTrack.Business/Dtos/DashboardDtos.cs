namespace RentalTrack.Business.Dtos;

public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int ActiveProperties { get; set; }
    public decimal TrackedMonthlyRent { get; set; }
    public int OverdueRentRows { get; set; }
}

public class RecentSignUpDto
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Provider { get; set; }
}

public class DashboardDto
{
    public DashboardStatsDto Stats { get; set; } = new();
    public List<RecentSignUpDto> RecentSignUps { get; set; } = new();
}
