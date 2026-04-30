namespace RentalTrack.Data.Dao;

public class DashboardStatsRow
{
    public int TotalUsers { get; set; }
    public int ActiveProperties { get; set; }
    public decimal TrackedMonthlyRent { get; set; }
    public int OverdueRentRows { get; set; }
}
